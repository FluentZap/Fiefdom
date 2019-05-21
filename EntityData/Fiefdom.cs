using System;
using System.Collections.Generic;
using Fiefdom.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Fiefdom
{

    public static class FiefdomActions
    {

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



        public static void CreateNewFiefdom(string name, string sessionId)
        {
            using (var db = new FiefContext())
            {
                Fief fief = new Fief { Name = name, SessionId = sessionId, Title = 0 };
                fief.FiefdomResources.Add(new FiefdomResources { Type = "Gold", Quantity = 200 });
                fief.FiefdomResources.Add(new FiefdomResources { Type = "Wood", Quantity = 10 });
                fief.FiefdomResources.Add(new FiefdomResources { Type = "Stone", Quantity = 10 });
                fief.FiefdomResources.Add(new FiefdomResources { Type = "Food", Quantity = 10 });
                if (db.Market.ToList().Count == 0)
                {
                    db.Market.Add(new Market { Type = "Wood", Price = 10 });
                    db.Market.Add(new Market { Type = "Food", Price = 10 });
                    db.Market.Add(new Market { Type = "Stone", Price = 10 });
                }
                for (int i = 0; i < 10; i++)
                {
                    fief.FiefdomPlot.Add(new FiefdomPlot { Type = "Locked" });
                }
                fief.FiefdomPlot[4].Type = "Empty";
                db.Fiefdom.Add(fief);
                db.SaveChanges();
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
                var price = db.Market.Where(w => w.Type == type).FirstOrDefault();
                int canBuy = gold.Quantity / price.Price;
                if (canBuy < quantity)
                {
                    quantity = canBuy;
                }
                gold.Quantity -= quantity * price.Price;
                buyType.Quantity += quantity;
                db.SaveChanges();
            }
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
                if (sellType.Quantity >= quantity)
                {
                    gold.Quantity += quantity * price.Price;
                    sellType.Quantity -= quantity;
                }

                db.SaveChanges();
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

        public static void UserUpdateSessionId(string name, string sessionId)
        {
            using (var db = new FiefContext())
            {
                var user = db.Fiefdom.Where(f => f.Name == name).FirstOrDefault();
                user.SessionId = sessionId;
                db.SaveChanges();
            }
        }


        public static void BuyQuanity(int Id, string name, int quantity)
        {
            using (var db = new FiefContext())
            {
                var fiefdom = db.Fiefdom.Where(f => f.Id == Id).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
                var gold = fiefdom.FiefdomResources.Where(t => t.Type == "Gold").FirstOrDefault();
                var item = fiefdom.FiefdomResources.Where(t => t.Type == name).FirstOrDefault();

                var marketItem = db.Market.Where(t => t.Type == name).FirstOrDefault();
                int cost = marketItem.Price * quantity;
                //Add market flux here
                item.Quantity += quantity;
                gold.Quantity -= cost;
                db.SaveChanges();
            }
        }

        public static void BuildPlot(string build, int id, int plot)
        {
            using (var db = new FiefContext())
            {
                var fiefdom = db.Fiefdom.Where(f => f.Id == id).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
                //Subtract resources
                fiefdom.FiefdomPlot.Where(p => p.Id == plot).FirstOrDefault().Type = build;
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

        public static Fief GetFiefdomBySessionId(String SessionId)
        {
            using (var db = new FiefContext())
            {
                var fiefdom = db.Fiefdom.Where(f => f.SessionId == SessionId).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
                return fiefdom;
            }
        }

        public static Fief GetFiefdomById(int Id)
        {
            using (var db = new FiefContext())
            {
                var fiefdom = db.Fiefdom.Where(f => f.Id == Id).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
                return fiefdom;
            }
        }

        public static Fief GetFiefdomByUserName(string userName)
        {
            using (var db = new FiefContext())
            {
                var fiefdom = db.Fiefdom.Where(f => f.Name == userName).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
                return fiefdom;
            }
        }
    }
}
