using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

namespace CloudEncryption.Encryption
{
    public class EncryptionHandler
    {
        // modifiable : privateKey = "yourstring"
        // if you change the default variables in this
        // file, you should change it in all it's occurences
        public static string AES_Encrypt(string userInput, string privateKey = "MyPassword")
        {
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(userInput);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(privateKey);
            byte[] encryptedBytes = null;
            // modifiable : saltBytes = new byte[] {...}
            byte[] saltBytes = new byte[] { 6, 9, 4, 2, 0, 7, 1, 1 };
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    // modifiable : KeySize, BlockSize
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    // modifiable : iterations
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;
                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
            return Convert.ToBase64String(encryptedBytes);
        }
        public static string AES_Decrypt(string userInput, string privateKey = "MyPassword")
        {
            byte[] bytesToBeDecrypted = Encoding.UTF8.GetBytes(userInput);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(privateKey);
            byte[] decryptedBytes = null;
            //
            byte[] saltBytes = new byte[] { 6, 9, 4, 2, 0, 7, 1, 1 };
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    //
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    //
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;
                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }
            return Convert.ToBase64String(decryptedBytes);
        }
        public static void EncryptFile(string sourceFilename, string destinationFilename, string password = "MyPassword", int iterations = 1042)
        {
            //
            byte[] salt = new byte[] { 6, 9, 4, 2, 0, 7, 1, 1 };
            AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            // NB: Rfc2898DeriveBytes initialization and subsequent calls to GetBytes must be eactly the same, including order, on both the encryption and decryption sides.
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, iterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);

            using (FileStream destination = new FileStream(destinationFilename, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using (CryptoStream cryptoStream = new CryptoStream(destination, transform, CryptoStreamMode.Write))
                {
                    using (FileStream source = new FileStream(sourceFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        source.CopyTo(cryptoStream);
                    }
                }
            }
        }
        public static void DecryptFile(string sourceFilename, string destinationFilename, string password = "MyPassword", int iterations = 1042)
        {
            //
            byte[] salt = new byte[] { 6, 9, 4, 2, 0, 7, 1, 1 };
            AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            // NB: Rfc2898DeriveBytes initialization and subsequent calls to   GetBytes   must be eactly the same, including order, on both the encryption and decryption sides.
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, iterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);

            using (FileStream destination = new FileStream(destinationFilename, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using (CryptoStream cryptoStream = new CryptoStream(destination, transform, CryptoStreamMode.Write))
                {
                    try
                    {
                        using (FileStream source = new FileStream(sourceFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            source.CopyTo(cryptoStream);
                        }
                    }
                    catch (CryptographicException exception)
                    {
                        if (exception.Message == "Padding is invalid and cannot be removed.")
                            throw new ApplicationException("Universal Microsoft Cryptographic Exception (Not to be believed!)", exception);
                        else
                            throw;
                    }
                }
            }
        }
    }
}
