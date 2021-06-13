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

        public static void Test()
        {
            string publicKey = File.ReadAllText(@"C:\Users\JiaHaoZhao\source\repos\WebApplication1\mykey.pem");
            string privateKey = File.ReadAllText(@"C:\Users\JiaHaoZhao\source\repos\WebApplication1\mykey.pub");

            var claims = new List<Claim>();
            claims.Add(new Claim("claim1", "value1"));
            claims.Add(new Claim("claim2", "value2"));
            claims.Add(new Claim("claim3", "value3"));

            var token = CreateToken(claims, privateKey);
            var payload = DecodeToken(token, publicKey);
        }

        public static string CreateToken(List<Claim> claims, string privateRsaKey)
        {
            RSAParameters rsaParams;
            using (var tr = new StringReader(privateRsaKey))
            {
                var pemReader = new PemReader(tr);
                var keyPair = pemReader.ReadObject() as AsymmetricCipherKeyPair;
                if (keyPair == null)
                {
                    throw new Exception("Could not read RSA private key");
                }
                var privateRsaParams = keyPair.Private as RsaPrivateCrtKeyParameters;
                rsaParams = DotNetUtilities.ToRSAParameters(privateRsaParams);
            }
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                Dictionary<string, object> payload = claims.ToDictionary(k => k.Type, v => (object)v.Value);
                return Jose.JWT.Encode(payload, rsa, Jose.JwsAlgorithm.RS256);
            }
        }

        public static string DecodeToken(string token, string publicRsaKey)
        {
             RSAParameters rsaParams;

             using (var tr = new StringReader(publicRsaKey))
             {
                  var pemReader = new PemReader(tr);
                  var publicKeyParams = pemReader.ReadObject() as RsaKeyParameters;
                  if (publicKeyParams == null)
                  {
                       throw new Exception("Could not read RSA public key");
                  }
                  rsaParams = DotNetUtilities.ToRSAParameters(publicKeyParams);
             }
             using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
             {
                  rsa.ImportParameters(rsaParams);
                  // This will throw if the signature is invalid
                  return Jose.JWT.Decode(token, rsa, Jose.JwsAlgorithm.RS256);  
             }
        }

        //public static string PrivateKey { get; set; }
        //public static AppData GetApplicationData()
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

        //public static void GetToken()
        //{
        //    AppData appData = GetApplicationData();

        //    var baseUrl = $"{appData.BaseAuthUrl}/auth/realms/stone_bank";
        //    var authUrl = $"{baseUrl}/protocol/openid-connect/token";

        //    string publicKey = File.ReadAllText(@"C:\Users\JiaHaoZhao\source\repos\WebApplication1\mykey.pem");
        //    string privateKey = File.ReadAllText(@"C:\Users\JiaHaoZhao\source\repos\WebApplication1\mykey.pub");

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

        //var payload = new Dictionary<string, object>
        //{
        //    { "exp", 123456 },
        //    { "nbf", 123 },
        //    { "aud",  baseUrl},
        //    { "realm", "stone_bank"},
        //    { "sub", appData.ClientId },
        //    { "clientId", appData.ClientId },
        //    { "jti", "time" },
        //    { "iat", 123456 }
        //};

        //const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

        //IJwtAlgorithm algorithm = new RS256Algorithm(privateKey); // symmetric
        //IJsonSerializer serializer = new JsonNetSerializer();
        //IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        //IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

        //var token = encoder.Encode(payload, secret);
        //Console.WriteLine(token);

    }
    }
