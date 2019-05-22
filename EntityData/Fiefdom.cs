using System;
using System.Collections.Generic;
using Fiefdom.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Fiefdom
{

    public class GameValues
    {
        public  List<string> Ballots = new List<string>{};
        public  List<Edict> Edicts = new List<Edict>{};
        public  int MarketTax{get; set; }
    }

    public class Edict
    {
        public bool Passed { get; set; }
        public string Type { get; set; }
        public string Target { get; set; }
        public string Amount { get; set; }
    }

    public static class FiefdomActions
    {
        public static List<string> Ballots = new List<string>{};
        public static List<Edict> Edicts = new List<Edict>{};
        public static int MarketTax{get; set; }

        static FiefdomActions() {
            MarketTax = 0;
        }

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
                fief.FiefdomResources.Add(new FiefdomResources { Type = "Gold", Quantity = 1000 });
                fief.FiefdomResources.Add(new FiefdomResources { Type = "Wood", Quantity = 15 });
                fief.FiefdomResources.Add(new FiefdomResources { Type = "Stone", Quantity = 15 });
                fief.FiefdomResources.Add(new FiefdomResources { Type = "Food", Quantity = 15 });                
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

         public static void SubmitVote(string sessionId, int ballot, string vote)
        {
            using (var db = new FiefContext())
            {
                var fiefdom = db.Fiefdom.Where(f => f.SessionId == sessionId).FirstOrDefault();
                if (ballot == 0) { fiefdom.Ballot1 = vote; }
                if (ballot == 1) { fiefdom.Ballot2 = vote; }
                if (ballot == 2) { fiefdom.Ballot3 = vote; }                
                db.SaveChanges();
            }
        }

        public static void ClearVote()
        {
            using (var db = new FiefContext())
            {
                foreach(Fief fief in db.Fiefdom.ToList())
                {
                    fief.Ballot1 = "vote"; 
                    fief.Ballot2 = "vote"; 
                    fief.Ballot3 = "vote";
                }
                              
                db.SaveChanges();
            }
        }

        public static List<bool> CountVotes()
        {
            List<bool> votes = new List<bool>{};
            using (var db = new FiefContext())
            {   
                Random rnd = new Random();
                var fiefdom = db.Fiefdom.Include("FiefdomPlot").Include("FiefdomResources").ToList();
                int ballot1 = 0;
                int ballot2 = 0;
                int ballot3 = 0;
                foreach(var fief in fiefdom)
                {
                    int influence = FiefdomActions.GetInfluence(fief);
                   
                    if (fief.Ballot1 == "Fore")
                    {
                        ballot1 += influence;
                    }else if(fief.Ballot1 == "Nay")
                    {
                        ballot1 -= influence;
                    }
                    if (fief.Ballot2 == "Fore")
                    {
                        ballot2 += influence;
                    }else if(fief.Ballot2 == "Nay")
                    {
                        ballot2 -= influence;
                    }
                    if (fief.Ballot3 == "Fore")
                    {
                        ballot3 += influence;
                    }else if(fief.Ballot3 == "Nay")
                    {
                        ballot3 -= influence;
                    }
                }
                if(ballot1 == 0)
                {
                    if(rnd.Next(1,1000) % 2 == 1)
                    {
                        ballot1++;
                    }else{
                        ballot1--;
                    }
                }
                if(ballot2 == 0)
                {
                    if(rnd.Next(1,1000) % 2 == 1)
                    {
                        ballot2++;
                    }else{
                        ballot2--;
                    }
                }
                if(ballot3 == 0)
                {
                    if(rnd.Next(1,1000) % 2 == 1)
                    {
                        ballot3++;
                    }else{
                        ballot3--;
                    }
                }
                votes.Add(ballot1 > 0);
                votes.Add(ballot2 > 0);
                votes.Add(ballot3 > 0);
                  
                db.SaveChanges();
            }
            return votes;
        }

        public static int GetInfluence(Fief fief)
        {
            int influence = 1;
            influence += fief.FiefdomResources.Where(r => r.Type == "Gold").FirstOrDefault().Quantity / 1000;
            influence += fief.Title * 10;

            return influence;
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


        // public static void BuyQuanity(int Id, string name, int quantity)
        // {
        //     using (var db = new FiefContext())
        //     {
        //         var fiefdom = db.Fiefdom.Where(f => f.Id == Id).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
        //         var gold = fiefdom.FiefdomResources.Where(t => t.Type == "Gold").FirstOrDefault();
        //         var item = fiefdom.FiefdomResources.Where(t => t.Type == name).FirstOrDefault();

        //         var marketItem = db.Market.Where(t => t.Type == name).FirstOrDefault();
        //         int cost = marketItem.Price * quantity;
        //         //Add market flux here
        //         item.Quantity += quantity;
        //         gold.Quantity -= cost;
        //         db.SaveChanges();
        //     }
        // }

        // public static void BuildPlot(string build, int id, int plot)
        // {
        //     using (var db = new FiefContext())
        //     {
        //         var fiefdom = db.Fiefdom.Where(f => f.Id == id).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
        //         //Subtract resources
        //         fiefdom.FiefdomPlot.Where(p => p.Id == plot).FirstOrDefault().Type = build;
        //     }
        // }

		 public static void BuildPlot(string sessionId, int id, string type)
        {
            using (var db = new FiefContext())
            {
				List<FiefdomResources> cost = new List<FiefdomResources>{
                    new FiefdomResources{Type = "Gold", Quantity = 500}, 
                    new FiefdomResources{Type = "Food", Quantity = 5}, 
                    new FiefdomResources{Type = "Stone", Quantity = 5}, 
                    new FiefdomResources{Type = "Wood", Quantity = 5}};
                Fief fief = db.Fiefdom.Where(f => f.SessionId == sessionId).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
				bool canAfford = true;
				foreach(var res in cost)
				{
					if(fief.FiefdomResources.Where(t => t.Type == res.Type).FirstOrDefault().Quantity < res.Quantity)
					{
						canAfford = false;
					}
				}
                //Subtract resources
				if(canAfford == true)
				{	
					if(fief.FiefdomPlot[id].Type == "Empty")
					{
						fief.FiefdomPlot[id].Type = type;
						foreach(var res in cost)
						{
							fief.FiefdomResources.Where(t => t.Type == res.Type).FirstOrDefault().Quantity -= res.Quantity;
						}
					}
				}
                
				db.SaveChanges();
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

        public static string CreateVote()
        {
            using (var db = new FiefContext())
            {
                Random rnd = new Random();
                List<string> keywordList = new List<string> {"Tax", "Market", "Levy"};
                string keyword = keywordList[rnd.Next(0,(keywordList.Count-1))];
                List<string> resourceList = new List<string>{"Wood", "Stone", "Food"};
                if (keyword == "Market" || keyword == "Levy")
                {
                    keyword += " " + resourceList[rnd.Next(0, 2)] + " " + rnd.Next(5,15);
                }                
                if (keyword == "Tax")
                {
                    keyword += " " + rnd.Next(5,15);                    
                }                
                return keyword;
            }
        }

    }
}
