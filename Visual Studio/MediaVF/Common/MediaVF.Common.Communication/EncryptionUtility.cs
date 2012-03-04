using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace MediaVF.Common.Communication
{
    public static class EncryptionUtility
    {
        public static string Encrypt(string key, string text)
        {
            // Our symmetric encryption algorithm
            AesManaged aes = new AesManaged();

            // We're using the PBKDF2 standard for password-based key generation
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes("password", Encoding.UTF8.GetBytes(key));

            // Setting our parameters
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;

            aes.Key = rfc.GetBytes(aes.KeySize / 8);
            aes.IV = rfc.GetBytes(aes.BlockSize / 8);

            // Encryption
            ICryptoTransform encryptTransf = aes.CreateEncryptor();

            // Output stream, can be also a FileStream
            MemoryStream encryptStream = new MemoryStream();
            CryptoStream encryptor = new CryptoStream(encryptStream, encryptTransf, CryptoStreamMode.Write);

            byte[] utfData = Encoding.Unicode.GetBytes(text);

            encryptor.Write(utfData, 0, utfData.Length);
            encryptor.Flush();
            encryptor.Close();

            // return encrypted content
            return Convert.ToBase64String(encryptStream.ToArray());
        }

        public static string Decrypt(string key, string encryptedText)
        {
            AesManaged aes = new AesManaged();

            // PBKDF2 standard for password-based key generation
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes("password", Encoding.UTF8.GetBytes(key));

            // set parameters
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;

            aes.Key = rfc.GetBytes(aes.KeySize / 8);
            aes.IV = rfc.GetBytes(aes.BlockSize / 8);

            // create decryptor
            ICryptoTransform decryptTrans = aes.CreateDecryptor();

            // get encrypted bytes from text
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            // decrypt to string
            string decryptedString = string.Empty;
            using (MemoryStream decryptStream = new MemoryStream())
            {
                // write decrypted to memory stream
                using (CryptoStream decryptor = new CryptoStream(decryptStream, decryptTrans, CryptoStreamMode.Write))
                {
                    decryptor.Write(encryptedBytes, 0, encryptedBytes.Length);
                }

                // get decrypted string from bytes
                byte[] decryptBytes = decryptStream.ToArray();
                decryptedString = UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
            }

            return decryptedString.Replace("\0", "");
        }

    }
}
