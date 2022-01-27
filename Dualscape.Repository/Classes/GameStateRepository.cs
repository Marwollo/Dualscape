using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Dualscape.Repository.Interfaces;

namespace Dualscape.Repository.Classes
{
    public class GameStateRepository : IGameStateRepository
    {
        public void CreateGameState(string gameID)
        {
            throw new NotImplementedException();
        }
    }
}
