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
        private IAmazonDynamoDB _dynamoDB;
        private string _tableName = "dualscape-gamedata";
        
        public GameStateRepository(IAmazonDynamoDB dynamoDB)
        {
            _dynamoDB = dynamoDB;
        }

        public async void CreateGameState(string gameID)
        {
            PutItemRequest putItemRequest = new PutItemRequest()
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "game-id", new AttributeValue { S = gameID  } },
                    { "state", new AttributeValue{ S = "10:10:100|300:10:100" } }
                }
            };
            await _dynamoDB.PutItemAsync(putItemRequest);
        }
    }
}
