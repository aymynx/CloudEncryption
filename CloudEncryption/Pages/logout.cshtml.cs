using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CloudEncryption.Pages
{
    public class logoutModel : PageModel
    {
        public void OnGet()
        {
            if (HttpContext.Session.GetString("userAuthenticated") != "true")
            {
                Response.Redirect("/Login");    
            }
            else
            {
                HttpContext.Session.Clear();
                Response.Redirect("/Login");
            }
        }
    }
}
