using System;
using System.Collections.Generic;
using Fiefdom.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Fiefdom
{

	public class GameValues
	{
		public List<string> Ballots = new List<string> { };
		public List<Edict> Edicts = new List<Edict> { };
		public int MarketTax { get; set; }
	}	


	public static partial class FiefdomActions
    {
		public static List<string> Ballots = new List<string> { };
		public static List<Edict> Edicts = new List<Edict> { };
		public static int MarketTax { get; set; }

		static FiefdomActions()
		{
			MarketTax = 0;
		}
		//Data Calls

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

        public static void UserUpdateSessionId(string name, string sessionId)
        {
            using (var db = new FiefContext())
            {
                var user = db.Fiefdom.Where(f => f.Name == name).FirstOrDefault();
                user.SessionId = sessionId;
                db.SaveChanges();
            }
        }
      
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
