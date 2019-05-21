using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Fiefdom.Context;
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
            Fief fief = FiefdomActions.GetFiefdomBySessionId(Context.ConnectionId);
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
            if (FiefdomActions.UserExist(name))
            {
                FiefdomActions.UserUpdateSessionId(name, Context.ConnectionId);
            }
            else
            {
                FiefdomActions.CreateNewFiefdom(name, Context.ConnectionId);
            }
            await Clients.All.SendAsync("ServerMessage", name + " joined the session");
        }

        public async Task BuyResource(string type, int quantity)
        {
            FiefdomActions.BuyResource(Context.ConnectionId, type, quantity);
        }

        public async Task SellResource(string type, int quantity)
        {
            FiefdomActions.SellResource(Context.ConnectionId, type, quantity);
        }


    }
}
