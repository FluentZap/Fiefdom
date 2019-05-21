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

namespace Fiefdom
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateWebHostBuilder(args).Build();
			ServerUpdate update = new ServerUpdate();
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
	}
}
