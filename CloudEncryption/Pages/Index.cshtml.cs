using CloudEncryption.FileHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Web;
using CloudEncryption.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Microsoft.JSInterop;

namespace CloudEncryption.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ILogger<IndexModel> _logger;
        private readonly IFileUploadHandler fileUploadHandler;
        public string FilePath;

        public IndexModel(ILogger<IndexModel> logger, IFileUploadHandler fileUploadHandler, IJSRuntime jsRuntime)
        {
            _logger = logger;
            _jsRuntime = jsRuntime;
            this.fileUploadHandler = fileUploadHandler;
        }
        public void OnGet()
        {
            if (HttpContext.Session.GetString("userAuthenticated") != "true")
            {
                Response.Redirect("/Login");
            }
        }
        // user_defined_key : privateKey = "yourstring"
        // if you change the default variables in this
        // project, you should change it in all it's occurences
        public async void OnPost(IFormFile file, bool to_encrypt ,string user_defined_key = "MyPassword")
        {
            if (file != null)
            {
                if(user_defined_key==null)
                    user_defined_key = "MyPassword";
                try
                {
                    FilePath = await fileUploadHandler.UploadFileAsync(file, HttpContext.Session.GetString("user_space"), to_encrypt, user_defined_key);
                    await _jsRuntime.InvokeAsync<object>("window", "Success", FilePath.Split('\\').Last() + " has been uploaded", "success");
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
