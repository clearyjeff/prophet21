using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Acme.P21.Common
{
    public static class Encryption
    {
        private static string EncryptionKey => 
            ConfigurationManager.AppSettings["EncryptionKey"] ?? 
            Environment.GetEnvironmentVariable("ENCRYPTION_KEY") ?? 
            throw new InvalidOperationException("Encryption key not found in configuration or environment variables");

        public static string Encrypt(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            using (var aes = Aes.Create())
            {
                aes.Key = DeriveKeyFromPassword(EncryptionKey);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor())
                {
                    var data = Encoding.UTF8.GetBytes(source);
                    var encrypted = encryptor.TransformFinalBlock(data, 0, data.Length);
                    
                    var result = new byte[aes.IV.Length + encrypted.Length];
                    Array.Copy(aes.IV, 0, result, 0, aes.IV.Length);
                    Array.Copy(encrypted, 0, result, aes.IV.Length, encrypted.Length);
                    
                    return Convert.ToBase64String(result);
                }
            }
        }

        public static string Decrypt(string encryptedData)
        {
            if (string.IsNullOrEmpty(encryptedData))
                return string.Empty;

            var data = Convert.FromBase64String(encryptedData);
            
            using (var aes = Aes.Create())
            {
                aes.Key = DeriveKeyFromPassword(EncryptionKey);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                
                var iv = new byte[aes.BlockSize / 8];
                Array.Copy(data, 0, iv, 0, iv.Length);
                aes.IV = iv;
                
                var encrypted = new byte[data.Length - iv.Length];
                Array.Copy(data, iv.Length, encrypted, 0, encrypted.Length);

                using (var decryptor = aes.CreateDecryptor())
                {
                    var decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
                    return Encoding.UTF8.GetString(decrypted);
                }
            }
        }

        private static byte[] DeriveKeyFromPassword(string password)
        {
            const int keySize = 32;
            const int iterations = 10000;
            var salt = Encoding.UTF8.GetBytes("Acme.P21.Salt.2024"); 
            
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(keySize);
            }
        }
    }
}