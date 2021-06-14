using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System.IO;
using System.Security.Claims;
using JWT.Builder;

namespace WebApplication1.Services
{
    public class AuthenticationService
    {


        public static AppData GetApplicationData()
        {
            AppData appData = new AppData()
            {
                ClientId = 646546546,
                BaseUrl = "",
                BaseAuthUrl = "",
                PrivateKey = ""
            };
            return appData;
        }

        public static void GetToken()
        {
            AppData appData = GetApplicationData();

            var baseUrl = $"{appData.BaseAuthUrl}/auth/realms/stone_bank";
            var authUrl = $"{baseUrl}/protocol/openid-connect/token";

            string publicKey = File.ReadAllText(@"C:\Users\JiaHaoZhao\source\repos\WebApplication1\mykey.pem");
            string privateKey = File.ReadAllText(@"C:\Users\JiaHaoZhao\source\repos\WebApplication1\mykey.pub");

            var token = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm()) // asymmetric
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                      .AddClaim("claim2", "claim2-value")
                      .Encode();
            // var payload = new Dictionary<string, object>
            // {
            //     {"userId", 1},
            //     { "exp", 123456 },
            // };



            // IJwtAlgorithm algorithm = new RS256Algorithm(); // asymmetric
            // IJsonSerializer serializer = new JsonNetSerializer();
            // IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            // IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            // var token = encoder.Encode(payload, secret);
            Console.WriteLine(token);

        }
    }
}