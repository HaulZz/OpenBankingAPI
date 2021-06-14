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
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.OpenSsl;
using JWT.Exceptions;
//using Tag.Library.SecretManager;

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
            string privateKey = File.ReadAllText(@"C:\Users\JiaHaoZhao\source\repos\WebApplication1\mykey.pem");

            var token = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                      .WithSecret(privateKey)
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                      .AddClaim("claim2", "claim2-value")
                      .Encode();

            Console.WriteLine(token);
            return token;
        }

        public static string DecodeTokenSHA256(string token)
        {
            //var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2MjM2Nzg3NjEsImNsYWltMiI6ImNsYWltMi12YWx1ZSJ9.h_EfnQFTRIZF9q41aAwWXnjPeF1Am3jjoAZOt11MqbA";
            string privateKey = File.ReadAllText(@"C:\Users\JiaHaoZhao\source\repos\WebApplication1\mykey.pem");
            var json = JwtBuilder.Create()
                     .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                     .WithSecret(privateKey)
                     .MustVerifySignature()
                     .Decode(token);
            Console.WriteLine(json);
            return json;
        }


        public static string GetTokenRS256()
        {

            var payload = new Dictionary<string, object>
            {
                { "claim1", 0 },
                { "claim2", "claim2-value" }
            };

            var header = new Dictionary<string, object>
            {
                { "{RS256}", "{JWT}" },
            };
            //var privateKey1 = _fixture.Create<RSACryptoServiceProvider>();
            string publicKey = File.ReadAllText(@"C:\Users\JiaHaoZhao\source\repos\WebApplication1\mykey.pub");
            string rsaPrivateKey = File.ReadAllText(@"C:\Users\JiaHaoZhao\source\repos\WebApplication1\mykey.pem");
            // new X509Certificate2(Encoding.ASCII.GetBytes(privateKey))
            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(GetRsaParameters(rsaPrivateKey));
            IJwtAlgorithm algorithm = new RS256Algorithm(csp,csp); // symmetric
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(header, payload, new byte[0]);
            Console.WriteLine(token);
            return token;
        }

        public static string DecodeTokenRS256(string token)
        {
            string rsaPublicKey = File.ReadAllText(@"C:\Users\JiaHaoZhao\source\repos\WebApplication1\mykey.pub");
            //const string token = "eyJ7UlMyNTZ9Ijoie0pXVH0iLCJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJjbGFpbTEiOjAsImNsYWltMiI6ImNsYWltMi12YWx1ZSJ9.LzhDVQ_evIcpikFqGbPFgU4yREsyGDX4njpFdtEXGJKdQErRuB4XqQcfnTS4hICat8uCaqkGUyg0XJEZgRkZHXXmLt35QDZrqbtA6GsKUBJNUJ0E9145jsmvzZ43OeXZ6Gl6XgKZZf7HAtoqqWmj9E_-PBOtu_tMY63Bw0VZ65fCquGuTM5Bsf8QGMXN5dQtWSujIwij3GmrqHIDFriZeHceolyCZ0ENVpI28bSswYUmDJTq6jGrp-Y_52JDUHpEGzKMgRHBG8cdK6u3KFO9CW0iv62G1NZlr79xII9mhR7ouo84NyOHrtkBDhrUJmFKPGnn8HNP-649vkwG6nBou-keKBYMD5Hd5l5Il6cIoy77lQosvpywATJTUHw9mrzlr085YK0p7bOtc4e1IlJqdnQ-TOWQpTPfrJu0-Fmr0ESdjaxnZk4hyaxB4ItINgWkX1twKL-d3k7M1-uPl9E9xMMVIZUPWlVl2hfRQ7vFc5Yz7GoTdbuH186xlcPjzfpHdrkflFq1BkFy6OjlFtxTsJ1AdH2NKEyCWunBrrtBEAIVJQoz3UJ1eDJqgAhIRzgyn6Hz75TBUIxxPUWTHUhBb1G2u6ZcTxCUU3WFR0wzYGCtm7GSnCPUvTDjEWzNKhV-jXhATQvfgndrLhil1iDtxHNsfVtIzem5j5kenKs39Qs";
            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(GetRsaPublicParameters(rsaPublicKey));
            var json = JwtBuilder.Create()
                //.WithAlgorithm(new RS256Algorithm(new X509Certificate2(Encoding.ASCII.GetBytes(rsaPublicKey)))) // asymmetric
                .WithAlgorithm(new RS256Algorithm(csp,csp)) // asymmetric    
                .MustVerifySignature()
                    .Decode(token);
            Console.WriteLine(json);
            return json;
        }

        public static string DecodeTokenRS2562(string token)
        {

            try
            {
                string rsaPublicKey = File.ReadAllText(@"C:\Users\JiaHaoZhao\source\repos\WebApplication1\mykey.pub");

                var csp = new RSACryptoServiceProvider();
                csp.ImportParameters(GetRsaPublicParameters(rsaPublicKey));
                IJsonSerializer serializer = new JsonNetSerializer();
                var provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new RS256Algorithm(csp,csp); // symmetric
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var json = decoder.Decode(token, Encoding.ASCII.GetBytes(rsaPublicKey), verify: true);
                Console.WriteLine(json);
                return json;
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
                return null;
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
                return null;
            }
            
        }

        private static RSAParameters GetRsaParameters(string rsaPrivateKey)
        {
            var byteArray = Encoding.ASCII.GetBytes(rsaPrivateKey);
            using (var ms = new MemoryStream(byteArray))
            {
                using (var sr = new StreamReader(ms))
                {
                    var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(sr);
                    var keyPair = pemReader.ReadObject() as AsymmetricCipherKeyPair;
                    return DotNetUtilities.ToRSAParameters(keyPair.Private as RsaPrivateCrtKeyParameters);
                }
            }
        }

        private static RSAParameters GetRsaPublicParameters(string rsaPrivateKey)
        {
            var byteArray = Encoding.ASCII.GetBytes(rsaPrivateKey);
            using (var ms = new MemoryStream(byteArray))
            {
                using (var sr = new StreamReader(ms))
                {
                    var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(sr);
                    var keyPair = pemReader.ReadObject() as AsymmetricCipherKeyPair;
                    return DotNetUtilities.ToRSAParameters(keyPair.Public as RsaKeyParameters);
                }
            }
        }

    }   
}
