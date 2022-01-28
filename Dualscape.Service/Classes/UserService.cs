using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dualscape.Domain.Models;
using Dualscape.Domain.Views;
using Dualscape.Repository.Interfaces;
using Dualscape.Service.Interfaces;

namespace Dualscape.Service.Classes
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public Task<User> Login(UserBaseView User)
        {
            _userRepository.Login(User);
            throw new NotImplementedException();
        }

        public Task<User> Register(UserBaseView User)
        {
            _userRepository.Register(User);
            throw new NotImplementedException();
        }
    }
}
