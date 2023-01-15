using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudEncryption.FileHandling
{
    public interface IFileUploadHandler
    {
        // modifiable : user_defined_key = "yourstring"
        // if you change the default variables in this
        // project, you should change it in all it's occurences
        public Task<string> UploadFileAsync(IFormFile file, string target_location ,bool to_encrypt, string user_defined_key = "MyPassword");
    }
}
