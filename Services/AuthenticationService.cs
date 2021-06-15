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
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using System.Security.Cryptography.X509Certificates;
using Jose;

namespace WebApplication1.Services
{
    public class AuthenticationService
    {
        public static string PrivateKeyPath = @"C:\Users\JiaHaoZhao\source\repos\WebApplication1\privatekey.pem";
        public static string PublicKeyPath = @"C:\Users\JiaHaoZhao\source\repos\WebApplication1\publickey.pub";


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
                //realm = "stone_bank", // melhorar isso? talvez
                //sub = "client_id", // todo:  obter nosso client_id
                //clientId = "client_id", // todo:  obter nosso client_id,
                //                        // jti = DateTime.UtcNow.Date,
                //                        //iat = DateTime.UtcNow.Date,
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

            var privateKey = GetPrivateKeyFromPemFile(PrivateKeyPath);

            IJwtAlgorithm algorithm = new RS256Algorithm(privateKey, privateKey);
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(header, payload, new byte[0]);
            Console.WriteLine(token);
            return token;
        }

        public static string CreateToken_jose()
        {
            var payload = new Dictionary<string, object>()
            {
                { "claim1", 2 },
                { "claim2", "claim2-value" }
            };

            //var privateKey = new X509Certificate2("privatekey.pem").GetRSAPrivateKey();
            var privateKey = GetPrivateKeyFromPemFile(PrivateKeyPath);
            string token = Jose.JWT.Encode(payload, privateKey, JwsAlgorithm.RS256);

            return token;
        }

        public static string DecodeTokenRS256(string token)
        {
            var publicKey = GetPublicKeyFromPemFile(PublicKeyPath);
            var privateKey = GetPrivateKeyFromPemFile(PrivateKeyPath);

            var json = JwtBuilder.Create()
                .WithAlgorithm(new RS256Algorithm(publicKey)) // asymmetric    
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
            IJwtAlgorithm algorithm = new RS256Algorithm(publicKey);
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

            var json = decoder.Decode(token, Encoding.ASCII.GetBytes(File.ReadAllText(PublicKeyPath)), verify: true);
            Console.WriteLine(json);
            return json;
        }

        public static string DecodeTokenRS256_jose(string token)
        {
            var publicKey = GetPublicKeyFromPemFile(PublicKeyPath);

            string json = Jose.JWT.Decode(token, publicKey, JwsAlgorithm.RS256);
            return json;
        }

        public static RSACryptoServiceProvider GetPrivateKeyFromPemFile(string PrivateKeyPath)
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

        public static RSACryptoServiceProvider GetPublicKeyFromPemFile(string PublicKeyPath)
        {
            using (TextReader publicKeyTextReader = new StringReader(File.ReadAllText(PublicKeyPath)))
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