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
using JWT.Builder;
using AutoFixture;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WebApplication1.Services
{
    public class Teste
    {

        //public static AppData GetApplicationDatas()
        //{
        //    AppData appData = new AppData()
        //    {
        //        ClientId = 646546546,
        //        BaseUrl = "",
        //        BaseAuthUrl = "",
        //        PrivateKey = ""
        //    };
        //    return appData;
        //}
        private static readonly Fixture _fixture = new Fixture();
        public static string GetTokenRS256()
        {

            var payload = new Dictionary<string, object>
            {
                { "claim1", 0 },
                { "claim2", "claim2-value" }
            };
            //var privateKey = _fixture.Create<RSACryptoServiceProvider>();
            string publicKey = File.ReadAllText(@"C:\Users\Haul\Documents\C#\OpenBankingAPI\mykey.pub");
            string privateKey = File.ReadAllText(@"C:\Users\Haul\Documents\C#\OpenBankingAPI\mykey.pem");

            IJwtAlgorithm algorithm = new RS256Algorithm(
                new X509Certificate2(
                Encoding.ASCII.GetBytes(privateKey))); // symmetric
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, privateKey);
            Console.WriteLine(token);
            return token;
        }

        public static string GetTokenSHA256()
        {
            const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
            //var payload = new Dictionary<string, object>
            //{
            //    { "claim1", 0 },
            //    { "claim2", "claim2-value" }
            //};


            //IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
            //IJsonSerializer serializer = new JsonNetSerializer();
            //IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            //IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            //var token = encoder.Encode(payload, secret);
            string privateKey = File.ReadAllText(@"C:\Users\Haul\Documents\C#\OpenBankingAPI\mykey.pem");

            var token = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                      .WithSecret(privateKey)
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                      .AddClaim("claim2", "claim2-value")
                      .Encode();

            Console.WriteLine(token);
            return token;
        }
        public static string DecodeTokenRS256()
        {
            string publicKey = File.ReadAllText(@"C:\Users\Haul\Documents\C#\OpenBankingAPI\mykey.pub");
            const string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJjbGFpbTEiOjAsImNsYWltMiI6ImNsYWltMi12YWx1ZSJ9.8pwBI_HtXqI3UgQHQ_rDRnSQRxFL1SR8fbQoS-5kM5s";
            var json = JwtBuilder.Create()
                    .WithAlgorithm(new RS256Algorithm(new X509Certificate2(
                Encoding.ASCII.GetBytes(publicKey)))) // asymmetric
                    .MustVerifySignature()
                    .Decode(token);
            Console.WriteLine(json);
            return json;
        }
       
    }   
}
