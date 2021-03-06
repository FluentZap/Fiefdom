﻿using Microsoft.AspNetCore.SignalR;
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
		
		public override Task OnConnectedAsync()
        {
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

		public async Task RequestFiefdomData()
        {			
			GameState gameState;
			List<Market> baseMarket;
			List<Market> buyPrice = new List<Market>();
			List<Market> sellPrice = new List<Market>();
			Fief fief;
			using (var db = new FiefContext())
			{
				gameState = db.GameState.FirstOrDefault();
				baseMarket = db.Market.ToList();								
				fief = db.Fiefdom.Where(f => f.SessionId == Context.ConnectionId).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
			}

			for (int i = 0; i < baseMarket.Count; i++)
			{
				buyPrice.Add(new Market
				{
					Type = baseMarket[i].Type,
					Price = FiefdomActions.GetMarketBuyPrice(baseMarket[i].Type, baseMarket[i].Price)
				});
				sellPrice.Add(new Market
				{
					Type = baseMarket[i].Type,
					Price = FiefdomActions.GetMarketSellPrice(baseMarket[i].Type, baseMarket[i].Price)
				});
			}

			if (fief != null)
            {
				await Clients.Caller.SendAsync("RecieveFiefdomData", fief, gameState,
						new GameValues
						{
							Ballots = FiefdomActions.Ballots,
							Edicts = FiefdomActions.Edicts,
							MarketTax = FiefdomActions.MarketTax,
							baseMarket = baseMarket,
							buyMarket = buyPrice,
							sellMarket = sellPrice
						});
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

		public async Task SubmitVote(int ballot, string vote)
        {
            FiefdomActions.SubmitVote(Context.ConnectionId, ballot, vote);
        }
		
		public async Task BuyResource(string type, int quantity)
        {			
            FiefdomActions.BuyResource(Context.ConnectionId, type, quantity);
        }

        public async Task SellResource(string type, int quantity)
        {			
			FiefdomActions.SellResource(Context.ConnectionId, type, quantity);
        }

		    public async Task BuyTitle()
        {			
			FiefdomActions.BuyTitle(Context.ConnectionId);
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
