using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class WebhookService
    {
        public static string Read(string encrypted)
        {
            var token = CryptographyService.Decrypt_jose(encrypted);
            var payload = CoderService.DecodeTokenRS256_jose(token);
            return payload;
        }
    }
}
