using Dualscape.Domain.Models;
using Dualscape.Domain.Views;
using Dualscape.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dualscape.API.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        [Route("register")]
        public IActionResult Register(UserBaseView User)
        {
        
            return new OkObjectResult(_userService.Register(User));
        }

        [Route("login")]
        public IActionResult Login(UserBaseView User)
        {

            return new OkObjectResult(_userService.Login(User));
        }
    }
}
