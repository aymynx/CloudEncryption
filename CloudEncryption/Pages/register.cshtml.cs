using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;
using CloudEncryption.Database;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Microsoft.JSInterop;


namespace CloudEncryption.Pages
{
    public class registerModel : PageModel
    {
        private readonly IJSRuntime _jsRuntime;
        public readonly IWebHostEnvironment environment;
        public registerModel(IWebHostEnvironment environment, IJSRuntime jsRuntime)
        {
            this.environment = environment;
            _jsRuntime = jsRuntime;
        }
        public void OnGet()
        {

        }
        public void OnPost(string username, string email, string pass) 
        {
            if (DatabaseManager.Register(DatabaseManager.CreateConnection(), username, email, pass))
            {
                HttpContext.Session.SetString("userAuthenticated", "true");
                HttpContext.Session.SetString("username", username);
                HttpContext.Session.SetString("email", email);
                try
                {
                    // modifiable : ..., "mystring", username
                    // you can change the name of the directory you want your
                    // users' data to be kept, just change it in all occurences
                    Directory.CreateDirectory(Path.Combine(environment.WebRootPath, "Data", username));
                    HttpContext.Session.SetString("user_space", Path.Combine(environment.WebRootPath, "Data", username));
                    HttpContext.Session.SetString("current_dir", "");
                }
                catch (Exception e)
                {
                    _jsRuntime.InvokeAsync<object>("window", "Error", "Cannot create account at the moment!", "error");
                    HttpContext.Session.Clear();
                    Response.Redirect("/Home");
                }
                try
                {
                    _jsRuntime.InvokeAsync<object>("window", "Success", "Your account has been created", "success");
                }catch(Exception e) { }
                Response.Redirect("/Index");
            }
            else // username exists
            {
                try
                {
                    _jsRuntime.InvokeAsync<object>("window", "Error", "Username already exists", "error");
                }catch(Exception e) { }
            }
        }

    }
}
