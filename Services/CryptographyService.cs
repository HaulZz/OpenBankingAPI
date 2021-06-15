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
using Jose;

//using Tag.Library.SecretManager;

namespace WebApplication1.Services
{
    public class CryptographyService
    {

        public static string PrivateKeyPath = @"C:\Users\JiaHaoZhao\source\repos\WebApplication1\privatekey.pem";
        public static string PublicKeyPath = @"C:\Users\JiaHaoZhao\source\repos\WebApplication1\publickey.pub";

        public static string Encrypt(string text)
        {
            //var text = "12345678";
            var publicKey = CoderService.GetPublicKeyFromPemFile(PublicKeyPath);
            Console.WriteLine(text);
            var encryptedBytes = publicKey.Encrypt(Encoding.UTF8.GetBytes(text), false);
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Encrypt_jose(string text)
        {
            var publicKey = CoderService.GetPublicKeyFromPemFile(PublicKeyPath);

            //string token = JWE.Encrypt(text, new[] { new JweRecipient(JweAlgorithm.RSA_OAEP_256, publicKey), }, JweEncryption.A256GCM, mode: SerializationMode.Compact);

            string token = Jose.JWT.Encode(text, publicKey, JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM);
            return token;
        }

        public static string Decrypt(string encrypted)
        {
            //var encrypte = "PTSEq/4FZq1WwGgy+1O23twL+0sb4YbJRISJR9Ch7V2S4bxcqdACKJpyFonaQBRrrYKKeNqwBDl0bWz0FP6WK9cSd0wGtJO2O+0px7JggUBQDmY2YStRXyvgomwVP+G962f26ylnzfs05cYrjlu/sjQkfYYxgpvAvTubR7RWlT4=";
            var privateKey = CoderService.GetPrivateKeyFromPemFile(PrivateKeyPath);
            var decryptedBytes = privateKey.Decrypt(Convert.FromBase64String(encrypted), false);
            return Encoding.UTF8.GetString(decryptedBytes, 0, decryptedBytes.Length);
        }

        public static string Decrypt_jose(string encrypted)
        {
              var privateKey = CoderService.GetPrivateKeyFromPemFile(PrivateKeyPath);

            //return JWE.Decrypt(encrypted, privateKey, JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM);
            return Jose.JWT.Decode(encrypted, privateKey, JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM);
        }
    }
}
