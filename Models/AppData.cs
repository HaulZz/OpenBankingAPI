using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class AppData
    {
        public int ClientId { get; set; }
        public string BaseUrl { get; set; }
        public string BaseAuthUrl { get; set; }
        public string PrivateKey { get; set; }
    }
}
