using System.Collections.Generic;
using Fiefdom.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Fiefdom
{

  public static class FiefdomAcions
  {
    public static void  CreateNewFiefdom(string Name, string SessionId)
    {
      using ( var db = new FiefContext())
      {
        Fief fief = new Fief {Name, SessionId};
        fief.FiefdomResources.Add(new FiefdomResources{ Type = "Gold", Quantity = 200});
        fief.FiefdomResources.Add(new FiefdomResources{ Type = "Wood", Quantity = 10});
        fief.FiefdomResources.Add(new FiefdomResources{ Type = "Stone", Quantity = 10});
        fief.FiefdomResources.Add(new FiefdomResources{ Type = "Food", Quantity = 10});
        for (int i = 0; i < 10; i++) {
          FiefdomPlot.Add(new FiefdomPlot{Type = "Empty"});
        }
        db.Fiefdom.Add(fief);
        db.SaveChanges();
      }
    }

    public static bool UserExist(string name)
    {
      using ( var db = new FiefContext())
      {
        var user = db.Fiefdom.Where(f => f.Name == name).FirstOrDefault();
        return user != null;
      }
    }

    public static void UserUpdateSessionId(string name, string sessionId)
    {
      using ( var db = new FiefContext())
      {
        var user = db.Fiefdom.Where(f => f.Name == name).FirstOrDefault();
        user.SessionId = sessionId;
        db.SaveChanges();
      }
    }


    public static void BuyQuanity(int Id, string name, int quantity)
    {
      using ( var db = new FiefContext())
      {
        var fiefdom = db.Fiefdom.Where(f => f.Id == Id).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
        var gold = fiefdom.FiefdomResources.Where(t => t.Type == "Gold").FirstOrDefault();
        var item = fiefdom.FiefdomResources.Where(t => t.Type == name).FirstOrDefault();

        var marketItem = db.Market.Where(t => t.Type == name).FirstOrDefault();
        int cost = marketItem.Price * quantity;
        //Add matket flux here
        item.Quantity += quantity;
        gold.Quantity -= cost;
        db.SaveChanges();
      }
    }

    public static void BuildPlot(string build, int id, int plot)
    {
      using ( var db = new FiefContext())
      {
        var fiefdom = db.Fiefdom.Where(f => f.Id == id).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
        //Subtract resources
        fiefdom.FiefdomPlot.Where( p => p.Id == plot).FirstOrDefault().Type = build;
      }
    }

    public static int GetPrice(string name)
    {
      using ( var db = new FiefContext())
      {
        var item = db.Market.Where(t => t.Type == name).FirstOrDefault();
        return item.Price;
      }
    }

    public static List<Market> GetMarketList()
    {
      using ( var db = new FiefContext())
      {
        var items = db.Market.ToList();

        return items;
      }
    }

    public static Fief GetFiefdomById(int Id)
    {
      using ( var db = new FiefContext())
      {
        var fiefdom = db.Fiefdom.Where(f => f.Id == Id).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
        return fiefdom;
      }
    }
  }
}
