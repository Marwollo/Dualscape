using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BCrypt.Net;
using Dualscape.Domain.Models;
using Dualscape.Domain.Views;
using Dualscape.Repository.Exceptions;
using Dualscape.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dualscape.Repository.Classes
{
    using BCrypt = BCrypt.Net.BCrypt;
    public class UserRepository : IUserRepository
    {
        private readonly IAmazonDynamoDB _dynamoDB;
        public UserRepository(IAmazonDynamoDB amazonDynamoDB)
        {
            _dynamoDB = amazonDynamoDB;
        }
        public async Task<string> LoginAsync(UserLoginView User)
        {
            Table usersTable = Table.LoadTable(_dynamoDB, "dualscape-users");

            ScanFilter scanFilter = new ScanFilter();
            scanFilter.AddCondition("email", ScanOperator.Equal, User.Email);

            Search search = usersTable.Scan(scanFilter);

            Document? result = (await search.GetNextSetAsync()).FirstOrDefault();


            if(result != null)
            {
                if(BCrypt.Verify(User.Password, result["password"]))
                {
                    return result["user-id"];
                }
                throw new Exception("Password is incorrect");
            }
                throw new Exception("User does not exist");
        }

        public async Task RegisterAsync(User User)
        {
            string hashedPass = BCrypt.HashPassword(User.Password);

            string Id = Guid.NewGuid().ToString();
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(User.Email);
            if(match.Success)
            {
                Dictionary<string, AttributeValue> attributes = new Dictionary<string, AttributeValue>();
                
                attributes["user-id"] = new AttributeValue { S = Id };
                attributes["email"] = new AttributeValue { S = User.Email};
                attributes["password"] = new AttributeValue { S = hashedPass };

                PutItemRequest request = new PutItemRequest
                {
                    TableName = "dualscape-users",
                    Item = attributes
                };
                await _dynamoDB.PutItemAsync(request);
            }
            else
            {
                throw new InvalidEmailException();
            }

        }

        public async Task<User> GetUserByEmail(string Email)
        {
            Dictionary<string, AttributeValue> key = new Dictionary<string, AttributeValue>
            {
                { "email", new AttributeValue { S = Email } }
            };

            GetItemRequest request = new GetItemRequest
            {
                TableName = "dualscape-users",
                Key = key,
            };

            var result = await _dynamoDB.GetItemAsync(request);
            return new User();
        }

    }
}
