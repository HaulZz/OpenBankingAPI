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
    public class WebhookServices
    {

        public static string PrivateKeyPath = @"C:\Users\JiaHaoZhao\source\repos\WebApplication1\privatekey.pem";
        public static string PublicKeyPath = @"C:\Users\JiaHaoZhao\source\repos\WebApplication1\publickey.pub";
        //public static RSACryptoServiceProvider PrivateKey = GetPrivateKeyFromPemFile(PrivateKeyPath);
        //public static RSACryptoServiceProvider PublicKey = GetPrivateKeyFromPemFile(PublicKeyPath);

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


        public static string CreateToken()
        {
            //var BASE_URL = SecretManagerContext.GetValue("TAG_APPLICATION_BANKACCOUNTAPI_SANDBOX_BASEBANK_URL");

            //verificar melhor maneira de armazenar chave 
            var header = new Dictionary<string, object>
        {
            { "{RS256}", "{JWT}" }
        };

            var privateKey = GetPrivateKeyFromPemFile(PrivateKeyPath);

            var algorithm = new RS256Algorithm(privateKey, privateKey);
            var serializer = new JsonNetSerializer();
            var urlEncoder = new JwtBase64UrlEncoder();
            var encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var payload = new
            {
               // exp = DateTime.UtcNow.AddMilliseconds(3600),
                //nbf = DateTime.UtcNow.Date,
                //aud = BASE_URL,
                realm = "stone_bank", // melhorar isso? talvez
                sub = "client_id", // todo:  obter nosso client_id
                clientId = "client_id", // todo:  obter nosso client_id,
               // jti = DateTime.UtcNow.Date,
                //iat = DateTime.UtcNow.Date,
                claim1 = 2,
                claim2 = "claim2-value"
            };

            var token = encoder.Encode(header, payload, new byte[0]);

            return token;
        }

        public static string CreateToken_2()
        {

            var payload = new Dictionary<string, object>
            {
                { "claim1", 2 },
                { "claim2", "claim2-value" }
            };

            var header = new Dictionary<string, object>
            {
                { "{RS256}", "{JWT}" },
            };
            //var privateKey1 = _fixture.Create<RSACryptoServiceProvider>();

            var privateKey = GetPrivateKeyFromPemFile(PrivateKeyPath);

            IJwtAlgorithm algorithm = new RS256Algorithm(privateKey, privateKey); // symmetric
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(header, payload, new byte[0]);
            Console.WriteLine(token);
            return token;
        }

        public static string Encrypt()
        {
            var text = "12345678";
            var publicKey = GetPublicKeyFromPemFile(PublicKeyPath);
            Console.WriteLine(text);
            var encryptedBytes = publicKey.Encrypt(Encoding.UTF8.GetBytes(text), false);
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt()
        {
            var encrypted = "PTSEq/4FZq1WwGgy+1O23twL+0sb4YbJRISJR9Ch7V2S4bxcqdACKJpyFonaQBRrrYKKeNqwBDl0bWz0FP6WK9cSd0wGtJO2O+0px7JggUBQDmY2YStRXyvgomwVP+G962f26ylnzfs05cYrjlu/sjQkfYYxgpvAvTubR7RWlT4=";
            var privateKey = GetPrivateKeyFromPemFile(PrivateKeyPath);
            var decryptedBytes = privateKey.Decrypt(Convert.FromBase64String(encrypted), false);
            return Encoding.UTF8.GetString(decryptedBytes, 0, decryptedBytes.Length);
        }

        public static string DecodeTokenRS256(string token)
        {
            var publicKey = GetPublicKeyFromPemFile(PublicKeyPath);

            var json = JwtBuilder.Create()
                .WithAlgorithm(new RS256Algorithm(publicKey, publicKey)) // asymmetric    
                .MustVerifySignature()
                .Decode(token);
            Console.WriteLine(json);
            return json;
        }

        public static string DecodeTokenRS256_2(string token)
        {
            var publicKey = GetPublicKeyFromPemFile(PublicKeyPath);

            IJsonSerializer serializer = new JsonNetSerializer();
            var provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm algorithm = new RS256Algorithm(publicKey, publicKey); 
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

            var json = decoder.Decode(token, Encoding.ASCII.GetBytes(File.ReadAllText(PublicKeyPath)), verify: true);
            Console.WriteLine(json);
            return json;
        }

        private static RSACryptoServiceProvider GetPrivateKeyFromPemFile(string PrivateKeyPath)
        {
            string rsaPrivateKey = File.ReadAllText(PrivateKeyPath);
            var byteArray = Encoding.ASCII.GetBytes(rsaPrivateKey);
            using (var ms = new MemoryStream(byteArray))
            {
                using (var sr = new StreamReader(ms))
                {
                    var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(sr);
                    var keyPair = pemReader.ReadObject() as AsymmetricCipherKeyPair;
                    RSAParameters rsaParams = DotNetUtilities.ToRSAParameters(keyPair.Private as RsaPrivateCrtKeyParameters);

                    RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                    csp.ImportParameters(rsaParams);
                    return csp;
                }
            }
        }

        private static RSACryptoServiceProvider GetPublicKeyFromPemFile(String filePath)
        {
            using (TextReader publicKeyTextReader = new StringReader(File.ReadAllText(filePath)))
            {
                RsaKeyParameters publicKeyParam = (RsaKeyParameters)new Org.BouncyCastle.OpenSsl.PemReader(publicKeyTextReader).ReadObject();

                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKeyParam);

                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                csp.ImportParameters(rsaParams);
                return csp;
            }
        }

    }   
}
