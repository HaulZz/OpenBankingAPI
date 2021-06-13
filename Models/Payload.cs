using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Payload
    {
        public int Exp { get; set; }
        public int Nbf { get; set; }
        public string Aud { get; set; }
        public string Realm { get; set; }
        public int Sub { get; set; }
        public int ClientId { get; set; }
        public string Jti { get; set; }
        public int Iat { get; set; }
    }
}

