using System;
using System.Collections.Generic;
using Fiefdom.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Fiefdom
{


	public static partial class FiefdomActions
	{
		//Market
		public static Dictionary<string, int> Transactions = new Dictionary<string, int> { { "Food", 0 }, { "Stone", 0 }, { "Wood", 0 } };

		public static void UnlockPlot(Fief fief)
		{
			int theta = 0;
			for (int x = 4; x >= 0 && x <= 9; x += theta)
			{
				if (fief.FiefdomPlot[x].Type == "Locked")
				{
					fief.FiefdomPlot[x].Type = "Empty";
					return;
				}
				theta += theta > 0 ? 1 : -1;
				theta = -theta;
			}
		}

		public static void BuyResource(string sessionId, string type, int quantity)
		{
			using (var db = new FiefContext())
			{
				var fiefdom = db.Fiefdom.Where(f => f.SessionId == sessionId).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
				var gold = fiefdom.FiefdomResources.Where(x => x.Type == "Gold").FirstOrDefault();
				var buyItem = fiefdom.FiefdomResources.Where(x => x.Type == type).FirstOrDefault();
				var marketPrice = db.Market.Where(w => w.Type == type).FirstOrDefault();

				int price = GetMarketBuyPrice(buyItem.Type, marketPrice.Price);
				//int price = GetMarketBuyPrice(buyItem.Type, 100);

				int canBuy = gold.Quantity / price;
				if (canBuy < quantity)
				{
					quantity = canBuy;
				}
				gold.Quantity -= quantity * price;				
				buyItem.Quantity += quantity;

				Transactions[buyItem.Type] += quantity;
				//Random rnd = new Random();
				//int diff = Math.Abs(price.Price - 10);
				//if (rnd.Next(1, 1000) % (10 - diff) > 1)
				//{
				//	price.Price++;
				//}
				db.SaveChanges();
			}
		}

		public static void SellResource(string sessionId, string type, int quantity)
		{
			// Add market fluctuations in this method
			using (var db = new FiefContext())
			{
				var fiefdom = db.Fiefdom.Where(f => f.SessionId == sessionId).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
				var gold = fiefdom.FiefdomResources.Where(x => x.Type == "Gold").FirstOrDefault();
				var sellItem = fiefdom.FiefdomResources.Where(x => x.Type == type).FirstOrDefault();
				var price = db.Market.Where(w => w.Type == type).FirstOrDefault();				

				if (quantity > sellItem.Quantity)
				{
					quantity = sellItem.Quantity;
				}

				gold.Quantity += quantity * GetMarketSellPrice(sellItem.Type, price.Price);
				//gold.Quantity += quantity * GetMarketSellPrice(sellItem.Type, 100);
				sellItem.Quantity -= quantity;

				Transactions[sellItem.Type] -= quantity;
				//Random rnd = new Random();
				//int diff = Math.Abs(price.Price - 10);
				//if (rnd.Next(1, 1000) % (10 - diff) > 1)
				//{
				//	price.Price--;
				//}
				db.SaveChanges();
			}
		}		

		public static int GetMarketSellPrice(string type, int cost)
		{
			int modifier = 0;
			foreach (Edict edict in Edicts)
			{
				if (edict.Type == "Market" && edict.Target == type && edict.Passed)
					modifier += edict.Amount;
			}

			return (int)(cost - cost * ((modifier + MarketTax) * .01));
		}

		public static int GetMarketBuyPrice(string type, int cost)
		{
			int modifier = 0;
			foreach (Edict edict in Edicts)
			{
				if (edict.Type == "Market" && edict.Target == type && edict.Passed)
				{
					modifier += edict.Amount;
				}
			}

			return (int)(cost - cost * ((modifier + MarketTax) * .01));
		}

		public static void BuyTitle(string sessionId)
		{
			using (var db = new FiefContext())
			{
				Fief fiefdom = db.Fiefdom.Where(f => f.SessionId == sessionId).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
				var gold = fiefdom.FiefdomResources.Where(x => x.Type == "Gold").FirstOrDefault();
				switch (fiefdom.Title)
				{
					case 0:
						if (gold.Quantity >= 1000)
						{
							fiefdom.Title = 1;
							gold.Quantity -= 1000;
							UnlockPlot(fiefdom);
							UnlockPlot(fiefdom);
						}
						break;
					case 1:
						if (gold.Quantity >= 5000)
						{
							fiefdom.Title = 2;
							gold.Quantity -= 5000;
							UnlockPlot(fiefdom);
							UnlockPlot(fiefdom);
						}
						break;
					case 2:
						if (gold.Quantity >= 10000)
						{
							fiefdom.Title = 3;
							gold.Quantity -= 10000;
							UnlockPlot(fiefdom);
							UnlockPlot(fiefdom);
						}
						break;
					case 3:
						if (gold.Quantity >= 20000)
						{
							fiefdom.Title = 4;
							gold.Quantity -= 20000;
							UnlockPlot(fiefdom);
							UnlockPlot(fiefdom);
						}
						break;
				}
				db.SaveChanges();

			}
		}

		public static List<Market> GetMarketPrice()
		{
			using (var db = new FiefContext())
			{
				return db.Market.ToList();
			}
		}

		public static bool UserExist(string name)
		{
			using (var db = new FiefContext())
			{
				var user = db.Fiefdom.Where(f => f.Name == name).FirstOrDefault();
				return user != null;
			}
		}

		public static int GetPrice(string name)
		{
			using (var db = new FiefContext())
			{
				var item = db.Market.Where(t => t.Type == name).FirstOrDefault();
				return item.Price;
			}
		}

		public static List<Market> GetMarketList()
		{
			using (var db = new FiefContext())
			{
				var items = db.Market.ToList();

				return items;
			}
		}

	}
}
