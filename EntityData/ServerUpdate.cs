using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fiefdom.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Fiefdom.Hubs;
using Microsoft.Extensions.Hosting;

namespace Fiefdom
{
	internal class ServerUpdate : IHostedService, IDisposable
	{

		private Timer updateTimer;
		private readonly IHubContext<FiefdomHub> _hubContext;



		public ServerUpdate(IHubContext<FiefdomHub> hubContext)
		{
			_hubContext = hubContext;
		}



		public void UpdateServer(object state)
		{
			using (var db = new FiefContext())
			{
				var fiefdom = db.Fiefdom.Include("FiefdomPlot").Include("FiefdomResources").ToList();
				foreach (Fief fief in fiefdom)
				{
					FiefdomUpdate.UpdateResources(fief);
				}
				var gameState = db.GameState.FirstOrDefault();

				gameState.Day++;
				if (gameState.Day >= 3)
				{
					ProcessVotes();
					gameState.Day = 1;
					gameState.Season += 1;
				}
				if (gameState.Season >= 4)
				{
					gameState.Season = 1;
					gameState.Year++;
				}
				db.SaveChanges();
				UpdateClients();
			}
		}


		public void ProcessVotes()
		{			
			FiefdomActions.Edicts.Clear();
			// Random rnd = new Random();
			List<bool> votes = FiefdomActions.CountVotes();
			for(int i = 0; i < FiefdomActions.Ballots.Count; i++)
			{	
				string edict = FiefdomActions.Ballots[i];
					String[] splitted = edict.Split();
					if(splitted[0] == "Market" || splitted[0] == "Levy")
					{
						FiefdomActions.Edicts.Add(new Edict{ Type = splitted[0], Target = splitted[1], Amount = splitted[2], Passed = votes[i]});
					}
					//immediate reduction
					if(splitted[0] == "Tax")
					{
						FiefdomActions.MarketTax = int.Parse(splitted[1]);
						FiefdomActions.Edicts.Add(new Edict{Type = "Tax", Amount = splitted[1], Passed = votes[i]});
					}
			}

			FiefdomActions.Ballots.Clear();
			FiefdomActions.ClearVote();
			FiefdomActions.Ballots.Add(FiefdomActions.CreateVote());
			FiefdomActions.Ballots.Add(FiefdomActions.CreateVote());
			FiefdomActions.Ballots.Add(FiefdomActions.CreateVote());
		}


		public async Task UpdateClients()
		{
			GameState gameState;
			List<Market> market;
			Fief fief;
			using (var db = new FiefContext())
			{
				gameState = db.GameState.FirstOrDefault();
				market = db.Market.ToList();

				foreach (string client in FiefdomUpdate.ConnectedUsers)
				{
					fief = db.Fiefdom.Where(f => f.SessionId == client).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
					if (fief != null)
					{
						await _hubContext.Clients.Client(client).SendAsync("RecieveFiefdomData", fief, gameState, market, 
						new GameValues { Ballots = FiefdomActions.Ballots, Edicts = FiefdomActions.Edicts, MarketTax = FiefdomActions.MarketTax });
					}
				}
			}
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			updateTimer = new Timer(UpdateServer, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			updateTimer?.Change(Timeout.Infinite, 0);
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			updateTimer?.Dispose();
		}
	}
}
