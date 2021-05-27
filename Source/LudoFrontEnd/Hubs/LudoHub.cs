using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoFrontEnd.Hubs
{
    public class LudoHub : Hub
    {
        public async Task SendMessage(string action, string data)
        {
            await Clients.All.SendAsync("ReceiveMessage", action, data);
        }
    }
}
