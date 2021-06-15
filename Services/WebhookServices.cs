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

        private static RSACryptoServiceProvider GetPublicKeyFromPemFile(string PublicKeyPath)
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
