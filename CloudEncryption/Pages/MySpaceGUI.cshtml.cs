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

namespace CloudEncryption.Pages
{
    public class MySpaceGUIModel : PageModel
    {
        private readonly ILogger<MySpaceGUIModel> _logger;

        public MySpaceGUIModel(ILogger<MySpaceGUIModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("userAuthenticated") != "true")
                Response.Redirect("/Login");
        }
    }
}

