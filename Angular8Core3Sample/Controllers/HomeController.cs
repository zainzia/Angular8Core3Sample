using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Angular8Core3Sample.Models.Identity;
using Angular8Core3Sample.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace Angular8Core3Sample.Controllers
{
    public class HomeController : Controller
    {

        private IHostingEnvironment _env;

        public HomeController(ApplicationDbContext context, 
                                RoleManager<IdentityRole> roleManager,
                                UserManager<ApplicationUser> userManager, 
                                IConfiguration configuration) 
        {
        }

        public IActionResult Index()
        {
            return View("~/ClientApp/src/index.html");
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
