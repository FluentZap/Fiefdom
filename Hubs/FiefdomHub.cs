using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Fiefdom.Context;
using Fiefdom;
using System.Linq;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Fiefdom.Hubs
{
    public class FiefdomHub : Hub
    {

		public static IHubCallerClients HubContext;
		
		public FiefdomHub ()
		{
			HubContext = Clients;
		}


		public override Task OnConnectedAsync()
        {
			HubContext = Clients;
			FiefdomUpdate.ConnectedUsers.Add(Context.ConnectionId);			
            return base.OnConnectedAsync();
        }

		public override async Task OnDisconnectedAsync(Exception exception)
		{			
			if (FiefdomUpdate.ConnectedUsers.Contains(Context.ConnectionId))
			{
				FiefdomUpdate.ConnectedUsers.Remove(Context.ConnectionId);
			}
			await base.OnDisconnectedAsync(exception);
		}

		public async Task UpdateClients()
		{					
			foreach (string client in FiefdomUpdate.ConnectedUsers)
			{
				Fief fief = FiefdomActions.GetFiefdomBySessionId(client);
				if (fief != null)
				{
					await Clients.Client(client).SendAsync("RecieveFiefdomData", fief.FiefdomPlot, fief.FiefdomResources, fief.Title);
				}
			}
		}

		public async Task RequestFiefdomData()
        {
			//int test = new FiefContext().FiefdomResources.Where(f => f.Id == 2).FirstOrDefault().Quantity;
			GameState gameState;
			List<Market> market;
			Fief fief;
			using (var db = new FiefContext())
			{
				gameState = db.GameState.FirstOrDefault();
				market = db.Market.ToList();
				fief = db.Fiefdom.Where(f => f.SessionId == Context.ConnectionId).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
			}			
            if (fief != null)
            {
				await Clients.Caller.SendAsync("RecieveFiefdomData", fief, gameState, market);				
            }
            else
            {				
                await Clients.Caller.SendAsync("RecieveFiefdomData", null);
            }
        }

        public async Task UserLogin(string name)
        {
            name = name.ToLower();
            if (FiefdomActions.UserExist(name))
            {
                FiefdomActions.UserUpdateSessionId(name, Context.ConnectionId);
            }
            else
            {
                FiefdomActions.CreateNewFiefdom(name, Context.ConnectionId);
            }
            await Clients.All.SendAsync("ServerMessage", name + " joined the session");
        }

        public async Task BuyResource(string type, int quantity)
        {
            FiefdomActions.BuyResource(Context.ConnectionId, type, quantity);
        }

        public async Task SellResource(string type, int quantity)
        {
            FiefdomActions.SellResource(Context.ConnectionId, type, quantity);
        }

		public async Task GetMarketPrice()
        {
			await Clients.Caller.SendAsync("ReceiveMarketPrices", FiefdomActions.GetMarketPrice());
        }
		public async Task BuildPlot(int id, string type)
        {
			
            FiefdomActions.BuildPlot(Context.ConnectionId, id, type);
        }

    }
}
