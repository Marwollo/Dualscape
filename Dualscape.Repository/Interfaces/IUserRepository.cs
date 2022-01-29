using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dualscape.Domain.Models;
using Dualscape.Domain.Views;

namespace Dualscape.Repository.Interfaces
{
    public interface IUserRepository
    {
        public Task RegisterAsync(User User);
        public Task<string> LoginAsync(UserLoginView User);
    }
}
