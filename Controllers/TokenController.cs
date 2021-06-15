using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Services;
using WebApplication1.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public ActionResult<string> Get()
        {
            return AuthenticationService.CreateToken_jose();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public ActionResult<string> Post(Token encriptedBody)
        {
            return AuthenticationService.DecodeTokenRS256_jose(encriptedBody.token);
        }

    }
}
