using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenBankingController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpPost]
        public ActionResult<string> Post(StringData data)
        {
            //var encrypted = "";
            return WebhookServices.Encrypt_jose(data.Data);
        }
    }
}
