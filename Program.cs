using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Fiefdom;
using Fiefdom.Context;

namespace Fiefdom
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateWebHostBuilder(args).Build();			
			NewGameInstance();
			host.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
			.ConfigureLogging(config =>
			{
				config.ClearProviders();
			})
			.UseUrls("http://0.0.0.0:5000")
			.UseStartup<Startup>();


		public static void NewGameInstance()
		{
			using (var db = new FiefContext())
			{
				if (db.GameState.FirstOrDefault() == null)
				{
					db.GameState.Add(new GameState { Day = 1, Season = 1, Year = 1 });
				}

				if (db.Market.ToList().Count == 0)
				{
					db.Market.Add(new Market { Type = "Wood", Price = 10 });
					db.Market.Add(new Market { Type = "Food", Price = 10 });
					db.Market.Add(new Market { Type = "Stone", Price = 10 });
				}
				db.SaveChanges();
			}
		}
	}
}
