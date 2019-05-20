using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Fiefdom.Models;
using Fiefdom;
using System.Linq;

namespace Fiefdom.Hubs
{
	public class FiefdomHub : Hub
	{
		public async Task RequestFiefdomData()
		{
			Console.WriteLine(Context.ConnectionId);
			int test = new FiefContext().FiefdomResources.Where(f => f.Id == 2).FirstOrDefault().Quantity;
			Fief fief = FiefdomAcions.GetFiefdomById(1);
			await Clients.All.SendAsync("RecieveFiefdomData", fief.FiefdomPlot, fief.FiefdomResources);
		}

		public async Task UserLogin(string name)
		{
			if (FiefdomAcions.UserExist())
			{
				FiefdomAcions.UserUpdateSessionId(name, Context.ConnectionId);
			}
			else
			{
				FiefdomAcions.CreateNewFiefdom(name, Context.ConnectionId);
			}

			await Clients.All.SendAsync("LoginUser");

			// Console.WriteLine(Context.ConnectionId);
			// int test = new FiefContext().FiefdomResources.Where(f => f.Id == 2).FirstOrDefault().Quantity;
			// Fief fief = FiefdomAcions.GetFiefdomById(1);
			// await Clients.All.SendAsync("RecieveFiefdomData", fief.FiefdomPlot, fief.FiefdomResources);
		}

		// public async Task RequestFiefdomData()
		// {
		// 	Console.WriteLine(Context.ConnectionId);
		// 	int test = new FiefContext().FiefdomResources.Where(f => f.Id == 2).FirstOrDefault().Quantity;
		// 	Fief fief = FiefdomAcions.GetFiefdomById(1);
		// 	await Clients.All.SendAsync("RecieveFiefdomData", fief.FiefdomPlot, fief.FiefdomResources);
		// }
	}
}
