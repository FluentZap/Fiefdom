using System.Collections.Generic;
using Fiefdom.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Fiefdom
{

  public class FiefdomAcions
  {


    public static void BuyQuanity(int Id, string name, int quanity)
    {
      using ( var db = new FiefContext())
      {
        var fiefdom = db.Fiefdom.Where(f => f.Id == Id).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
        var gold = fiefdom.FiefdomResources.Where(t => t.Type == "Gold").FirstOrDefault();
        var item = fiefdom.FiefdomResources.Where(t => t.Type == name).FirstOrDefault();

        var marketItem = db.Market.Where(t => t.Type == name).FirstOrDefault();
        int cost = marketItem.Price * quanity;
        //Add matket flux here
        item.Quanity += quanity;
        gold.Quanity -= cost;
        db.SaveChanges();
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
