using Dualscape.Domain.Models;
using Dualscape.Domain.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dualscape.Service.Interfaces
{
    public interface IUserService
    {
        public Task<User> Register(UserBaseView User);
        public Task<User> Login(UserBaseView User);
    }
}
