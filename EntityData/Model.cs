using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Fiefdom.Models
{

  public class FdContext : DbContext
  {
    public DbSet<GameState> GameState { get; set; }
    public DbSet<Market> Market { get; set; }
    public DbSet<Fiefdom> Fiefdom { get; set; }
    public DbSet<FiefdomResources> FiefdomResources { get; set; }
    public DbSet<FiefdomPlot> FiefdomPlot { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseMySQL("server=localhost;database=fiefdom;user=root;password=root;port=8889;");
    }
  }

  public class GameState
  {
    public int Id{ get; set; }
    public int CurrentDay{ get; set; }
  }

  public class Market
  {
    [Key][MaxLength(255)]
    public string Type { get; set; }
    public int Price{ get; set; }
  }

  public class Fiefdom
  {
    public int Id{ get; set; }
    public string Name{ get; set; }
    public string SessionId{ get; set; }
    public List<FiefdomResources> FiefdomResources { get; set; }
    public List<FiefdomPlot> FiefdomPlot { get; set; }
  }

  public class FiefdomResources
  {
    public int Id{ get; set; }
    public string Type{ get; set; }
    public string Quanity{ get; set; }
    public int FiefdomId{ get; set; }
  }

  public class FiefdomPlot
  {
    public int Id{ get; set; }
    public string Type{ get; set; }
    public int FiefdomId{ get; set; }
  }

}
