using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using CloudEncryption.Encryption;
using System.Net.Http.Formatting;
using Microsoft.AspNetCore.Mvc;

namespace CloudEncryption.FileHandling
{
    public class LocalFileHandler : IFileUploadHandler
    {
        // modifiable : ext = "yourstring"
        // this is the new extention of the new 
        // and encrypted file, it can be left empty
        // just not null
        public readonly string ext = ".000";
        public readonly IWebHostEnvironment environment;
        public LocalFileHandler(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        // user_defined_key : privateKey = "yourstring"
        // if you change the default variables in this
        // project, you should change it in all it's occurences
        public async Task<string> UploadFileAsync(IFormFile file, string target_location, bool to_encrypt, string user_defined_key = "MyPassword")
        {
            var filePath = Path.Combine(target_location, file.FileName);
            using var filestream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(filestream);
            filestream.Dispose();
            if(to_encrypt)
            {
                Encrypt(filePath, user_defined_key);
                File.Delete(filePath);
                return (new  (filePath));
            }
            return filePath;
        }
        public static string[] GetUserSpaceFiles(string location)
        {
            return Directory.GetFiles(location, "*.*", SearchOption.AllDirectories);
        }
        public static void Encrypt(string filename, string user_defined_key = "MyPassword", string ext = ".000")
        {
            EncryptionHandler.EncryptFile(filename, filename+ext, EncryptionHandler.AES_Encrypt(user_defined_key));
        }
        public static void Decrypt(string filename, string user_defined_key = "MyPassword", string ext = ".000")
        {
            EncryptionHandler.DecryptFile(filename, filename.Replace(ext, ""), EncryptionHandler.AES_Encrypt(user_defined_key));
        }
        public static void DeleteFile(string file)
        {
            File.Delete(file);
        }
    }
}
