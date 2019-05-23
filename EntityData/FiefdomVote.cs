using System;
using System.Collections.Generic;
using Fiefdom.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Fiefdom
{

	public class Edict
	{
		public bool Passed { get; set; }
		public string Type { get; set; }
		public string Target { get; set; }
		public int Amount { get; set; }
	}

	public static partial class FiefdomActions
    {
		//Voting

        public static int GetInfluence(Fief fief)
        {
            int influence = 1;
            influence += fief.FiefdomResources.Where(r => r.Type == "Gold").FirstOrDefault().Quantity / 1000;
            influence += fief.Title * 10;

            return influence;
        }

        public static string CreateVote()
        {
            using (var db = new FiefContext())
            {
                Random rnd = new Random();
                List<string> keywordList = new List<string> {"Tax", "Market"};
                string keyword = keywordList[rnd.Next(0, keywordList.Count)];
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
    }
}
