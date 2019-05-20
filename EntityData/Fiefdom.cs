using System.Collections.Generic;
using Fiefdom.Models;
using System.Linq;

namespace Fiefdom
{

  public class FiefdomAcions
  {


    public static int BuyQuanity(string name, int quanity)
    {
      using ( var db = new FdContext())
      {
        var fiefdom = db.Fiefdom.Where(f => f.Id == Id).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
        var gold = fiefdom.FiefdomResources.List.Where(t => t.Type == "Gold").FirstOrDefault();
        var item = fiefdom.FiefdomResources.List.Where(t => t.Type == name).FirstOrDefault();

        var marketItem = db.Market.Where(t => t.Type == name).FirstOrDefault();
        int cost = marketItem.Price * quanity;
        //Add matket flux here
        item.Quanity += quanity;
        gold.quanity -= cost;
        db.SaveChanges();
        return item.Price;
      }
    }

    public static int GetPrice(string name)
    {
      using ( var db = new FdContext())
      {
        var item = db.Market.Where(t => t.Type == name).FirstOrDefault();
        return item.Price;
      }
    }

    public static List<Market> GetMarketList()
    {
      using ( var db = new FdContext())
      {
        var items = db.Market.ToList();

        return items;
      }
    }

    public static Fiefdom GetFiefdomById(int Id)
    {
      using ( var db = new FdContext())
      {
        var fiefdom = db.Fiefdom.Where(f => f.Id == Id).Include("FiefdomPlot").Include("FiefdomResources").FirstOrDefault();
        return fiefdom;
      }
    }
  }
}
