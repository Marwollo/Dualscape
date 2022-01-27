using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dualscape.Repository.Interfaces
{
    public interface IGameStateRepository
    {
        void CreateGameState(string gameID);
    }
}
