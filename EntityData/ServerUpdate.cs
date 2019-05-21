using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Fiefdom.Context;
using Microsoft.EntityFrameworkCore;

namespace Fiefdom
{
	public class ServerUpdate
	{

		private static System.Timers.Timer updateTimer;


		public ServerUpdate()
		{
			updateTimer = new System.Timers.Timer(5000);
			// Hook up the Elapsed event for the timer.
			updateTimer.Elapsed += UpdateServer;
			updateTimer.AutoReset = true;
			updateTimer.Enabled = true;
		}


		public void UpdateServer(Object source, ElapsedEventArgs e)
		{			
			using (var db = new FiefContext())
			{
				var fiefdom = db.Fiefdom.Include("FiefdomPlot").Include("FiefdomResources").ToList();
				foreach (Fief fief in fiefdom)
				{
					FiefdomUpdate.UpdateResources(fief);
				}
				db.SaveChanges();
			}
		}		
	}
}
