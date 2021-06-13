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

namespace WebApplication1.Services
{
    public class AuthenticationService
    {
        public AuthenticationService()
        {
            PrivateKey = "-----BEGIN RSA PRIVATE KEY-----" +
                "Insira aqui sua private_key" +
                " END RSA PRIVATE KEY---- - ";
        }



        public static string PrivateKey { get; set; }
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

            //var payload = new Payload()
            //{
            //    ClientId = appData.ClientId,
            //    Exp = 321321,
            //    Nbf = 32132132,
            //    Aud = baseUrl,
            //    Realm = "Stone_bank",
            //    Sub = appData.ClientId,
            //    Jti = "tempo",
            //    Iat = 321321
            //};

            //var token = jwt.encode(payload, app_data.private_key, algorithm='RS256')

            //object header =
            //{
            //    "Content-Type" : "application/x-www-form-urlencoded",
            //    "User-Agent": "Insira aqui o Nome da Aplicação"
            //};

            var payload = new Dictionary<string, object>
            {
                { "exp", 123456 },
                { "nbf", 123 },
                { "aud",  baseUrl},
                { "realm", "stone_bank"},
                { "sub", appData.ClientId },
                { "clientId", appData.ClientId },
                { "jti", "time" },
                { "iat", 123456 }
            };

            const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

            IJwtAlgorithm algorithm = new RS256Algorithm(privateKey); // symmetric
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, secret);
            Console.WriteLine(token);

        }
    }
