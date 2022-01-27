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
        public async Task UpdateGameState(string delta)
        {
            await Clients.All.SendAsync("GameStateUpdate", delta);
        }
    }
}
