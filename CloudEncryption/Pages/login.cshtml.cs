using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;
using CloudEncryption.Database;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.JSInterop;
using System;

namespace CloudEncryption.Pages
{
    public class loginModel : PageModel
    {
        public readonly IWebHostEnvironment environment;
        private readonly IJSRuntime _jsRuntime;
        public loginModel(IWebHostEnvironment environment, IJSRuntime jsRuntime)
        {
            this.environment = environment;
            _jsRuntime = jsRuntime;
        }
        public bool wrong_password = false;
        public void OnGet()
        {

        }
        public void OnPost(string username, string password) 
        {
            if(DatabaseManager.CheckUser(DatabaseManager.CreateConnection(), username))
            {
                if (DatabaseManager.CheckPassword(DatabaseManager.CreateConnection(), username, password))
                {
                    HttpContext.Session.SetString("userAuthenticated", "true");
                    HttpContext.Session.SetString("username", username);
                    HttpContext.Session.SetString("email", DatabaseManager.GetEmail(DatabaseManager.CreateConnection(), username));
                    HttpContext.Session.SetString("user_space", Path.Combine(environment.WebRootPath, "Data", username));
                    HttpContext.Session.SetString("current_dir", "");
                    try
                    {
                        _jsRuntime.InvokeAsync<object>("window", "Success", "You are being logged in", "success");

                    }catch(Exception e) { }
                    Response.Redirect("/Index");
                } else { // wrong pass
                    try
                    {
                        _jsRuntime.InvokeAsync<object>("window", "Error", "Wrong password", "error");
                    }
                    catch (Exception e) { }
                }
            } else { // wrong username
                try
                {
                    _jsRuntime.InvokeAsync<object>("window", "Error", "No such username", "error");
                }
                catch (Exception e) { }
            }
        }

    }
}
