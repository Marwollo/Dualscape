using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dualscape.Domain.Models;
using Dualscape.Domain.Views;
using Dualscape.Repository.Interfaces;
using Dualscape.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Dualscape.Service.Classes
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _config = configuration;
        }
        public async Task<string> LoginAsync(UserLoginView User)
        {
            string UserId = await _userRepository.LoginAsync(User);
            return GenerateTokenPair(UserId);
        }

        public async Task RegisterAsync(User User)
        {
            try{
                await _userRepository.RegisterAsync(User);
            } catch {
                throw new Exception("User is not registred"); 
            }
        }

        public string GenerateTokenPair(string uuid)
        {
            JwtSecurityTokenHandler jwtHandler = new();
            
            byte[] key = Encoding.ASCII.GetBytes(_config.GetSection("Secrets").GetSection("Jwt").Value);

            SecurityTokenDescriptor accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, uuid) }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken accessToken = jwtHandler.CreateToken(accessTokenDescriptor);

            return jwtHandler.WriteToken(accessToken);
        }
    }
}
