using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudEncryption.Encryption;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CloudEncryption.Pages
{
    public class MySpaceModel : PageModel
    {
        private readonly ILogger<MySpaceModel> _logger;
        [BindProperty(SupportsGet = true)]
        public string arg { get; set; }
        public MySpaceModel(ILogger<MySpaceModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            if (HttpContext.Session.GetString("userAuthenticated") != "true")
                Response.Redirect("/Login");
        }
        public void OnPost()
        {

        }
        public IActionResult OnPostDownloadFile(string filePath)
        {
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, "application/force-download", filePath.Split('\\').Last());
        }
        public void OnPostEncryptFile(string filePath, string user_defined_key)
        {
            if (user_defined_key is null)
            {
                // modifiable : state
                // you change whether you want to keep the original copy of the encrypted file
                FileHandling.LocalFileHandler.Encrypt(filePath, "MyPassword");
                OnPostDeleteFile(filePath);
            }
            else
            {

                FileHandling.LocalFileHandler.Encrypt(filePath, user_defined_key);
                OnPostDeleteFile(filePath);
            }
        }
        public void OnPostDecryptFile(string filePath, string user_defined_key)
        {
            if(user_defined_key is null)
            {
                // modifiable : state
                // you change whether you want to keep the original copy of the decrypted file
                FileHandling.LocalFileHandler.Decrypt(filePath, "MyPassword");
                OnPostDeleteFile(filePath);
            }
            else
            {
                FileHandling.LocalFileHandler.Decrypt(filePath, user_defined_key);
                OnPostDeleteFile(filePath);
            }
        }
        public void OnPostDeleteFile(string filePath)
        {
            FileHandling.LocalFileHandler.DeleteFile(filePath);
        }
    }
}

