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
using System.Security.Cryptography;
using ThirdParty.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using AutoFixture;
using JWT.Builder;

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

            var payload = new Payload()
            {
                ClientId = appData.ClientId,
                Exp = 321321,
                Nbf = 32132132,
                Aud = baseUrl,
                Realm = "Stone_bank",
                Sub = appData.ClientId,
                Jti = "tempo",
                Iat = 321321
            };

            var token = jwt.encode(payload, app_data.private_key, algorithm = 'RS256')

        object header =
        {
            "Content-Type" : "application/x-www-form-urlencoded",
            "User-Agent": "Insira aqui o Nome da Aplicação"
        };

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
        private static readonly Fixture _fixture = new Fixture();
        var publicKey = _fixture.Create<RSACryptoServiceProvider>();

        var token = JwtBuilder.Create()
                              .WithAlgorithm(new RS256Algorithm(publicKey, privateKey)) // symmetric
                              .WithSecret(secret)
                              .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                              .AddClaim("claim2", "claim2-value")
                              .Encode();

        Console.WriteLine(token);

    }
    }
