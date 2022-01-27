using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;


namespace Dualscape.RTC.Hubs
{
    public class GameStateHub : Hub
    {
        public async void UpdatePosition(string buffer)
        {
            await Clients.All.SendAsync("up", buffer);
        }
    }
}
