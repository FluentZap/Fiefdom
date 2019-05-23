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

		public static void UnlockPlot(Fief fief)
        {
            int theta = 0;
            for (int x = 4; x >= 0 && x <= 9; x += theta )
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
            // Add market fluctuations in this method

            using (var db = new FiefContext())
            {
               var fiefdom = db.Fiefdom.Where(f => f.SessionId == sessionId).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
                var gold = fiefdom.FiefdomResources.Where(x => x.Type == "Gold").FirstOrDefault();
                var buyType = fiefdom.FiefdomResources.Where(x => x.Type == type).FirstOrDefault();
                var price = (db.Market.Where(w => w.Type == type).FirstOrDefault());

                double taxedPrice = price.Price + price.Price * FiefdomActions.GetMarketTaxRate(buyType.Type);
                
                int canBuy = (int)(gold.Quantity / taxedPrice);
                if (canBuy < quantity)
                {
                  quantity = canBuy;
                }
                gold.Quantity -= (int)(quantity * taxedPrice);
                buyType.Quantity += quantity;
                Random rnd = new Random();
                int diff = Math.Abs(price.Price - 10);
                if(rnd.Next(1,1000) % (10 - diff) > 1)
                {
                  price.Price++;
                }
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
              var sellType = fiefdom.FiefdomResources.Where(x => x.Type == type).FirstOrDefault();
              var price = db.Market.Where(w => w.Type == type).FirstOrDefault();
              int sellTotal = price.Price * quantity;
              if(sellType.Quantity >= quantity)
              {
                gold.Quantity += (int)(quantity * price.Price * GetMarketTaxRate(sellType.Type));
                sellType.Quantity -= quantity;
              }

              Random rnd = new Random();
              int diff = Math.Abs(price.Price - 10);
              if(rnd.Next(1,1000) % (10 - diff) > 1)
              {
                price.Price--;
              }
              db.SaveChanges();
            }
        }

        public static double GetMarketTaxRate(){
            int tax = 0;
            foreach(Edict edict in FiefdomActions.Edicts)
            {
                if(edict.Type == "Tax")
                tax += int.Parse(edict.Amount);
            }

            return tax * .01;
        }

        public static double GetMarketTaxRate(string type) {
            int tax = 0;
            foreach(Edict edict in FiefdomActions.Edicts)
            {
                if(edict.Type == "Market" && edict.Target == type)
                tax += int.Parse(edict.Amount);
            }

            return (tax + GetMarketTaxRate()) * .01;
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
