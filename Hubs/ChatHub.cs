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

		public override Task OnConnectedAsync()
		{
			Console.WriteLine(Context.User.Identity.Name);
			return base.OnConnectedAsync();
		}

		public async Task RequestFiefdomData()
		{
			//int test = new FiefContext().FiefdomResources.Where(f => f.Id == 2).FirstOrDefault().Quantity;
			Fief fief = FiefdomAcions.GetFiefdomBySessionId(Context.ConnectionId);
			if (fief != null)
			{
				await Clients.Caller.SendAsync("RecieveFiefdomData", fief.FiefdomPlot, fief.FiefdomResources);
			}
			else
			{
				await Clients.Caller.SendAsync("RecieveFiefdomData", null);
			}
		}

		public async Task UserLogin(string name)
		{
			name = name.ToLower();
			if (FiefdomAcions.UserExist(name))
			{
				FiefdomAcions.UserUpdateSessionId(name, Context.ConnectionId);
			}
			else
			{
				FiefdomAcions.CreateNewFiefdom(name, Context.ConnectionId);
			}
			await Clients.All.SendAsync("ServerMessage", name + " joined the session");
		}
	}
}
