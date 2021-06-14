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
    public class WebhookController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Teste.GetTokenRS256();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return Teste.GetTokenSHA256();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public ActionResult<string> Post(Token encriptedBody)
        {
            //return Teste.DecodeTokenRS256();
            return Teste.DecodeTokenRS2562(encriptedBody.encrypted_body);
        }

    }
}
