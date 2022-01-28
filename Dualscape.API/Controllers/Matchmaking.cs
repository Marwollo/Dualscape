using Dualscape.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dualscape.API.Controllers
{
    [ApiController]
    [Route("matchmaking")]
    public class Matchmaking : Controller
    {
        [HttpGet("start")]
        public IActionResult Start()
        {

            return new OkObjectResult("Hello world!");
        }
        [HttpGet("nesto")]
        public IActionResult Nesto()
        {
            return new OkObjectResult("Ovo je nesto");
        }

    }
}
