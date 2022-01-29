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
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(User User)
        {
            await _userService.RegisterAsync(User);
            return new OkObjectResult("User is registred");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginView User)
        {
            return new OkObjectResult(await _userService.LoginAsync(User));
        }
    }
}
