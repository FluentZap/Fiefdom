using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Fiefdom.Context
{

	public class FiefContext : DbContext
	{
		public DbSet<GameState> GameState { get; set; }
		public DbSet<Market> Market { get; set; }
		public DbSet<Fief> Fiefdom { get; set; }
		public DbSet<FiefdomResources> FiefdomResources { get; set; }
		public DbSet<FiefdomPlot> FiefdomPlot { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySQL("server=localhost;database=fiefdom;user=root;password=root;port=8889;");
		}		
	}

	public class GameState
	{
		public int Id { get; set; }
		public int Day { get; set; }
		public int Season { get; set; }
		public int Year { get; set; }		
	}

	public class Market
	{
		[Key]
		[MaxLength(255)]
		public string Type { get; set; }
		public int Price { get; set; }
	}

	public class Fief
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string SessionId { get; set; }
		public int Title { get; set; }
		public string Ballot1 { get; set; }
		public string Ballot2 { get; set; }
		public string Ballot3 { get; set; }
		public List<FiefdomResources> FiefdomResources { get; set; } = new List<FiefdomResources>();
		public List<FiefdomPlot> FiefdomPlot { get; set; } = new List<FiefdomPlot>();
	}

	public class FiefdomResources
	{
		public FiefdomResources()
		{
			Id = 0;
		}

		public int Id { get; set; }
		public string Type { get; set; }
		public int Quantity { get; set; }
	}

	public class FiefdomPlot
	{
		public int Id { get; set; }
		public string Type { get; set; }
	}

}
