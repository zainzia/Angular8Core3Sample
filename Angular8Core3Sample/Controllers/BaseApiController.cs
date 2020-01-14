
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using Angular8Core3Sample.Data;
using Angular8Core3Sample.Models.Identity;

using Newtonsoft.Json;

namespace Angular8Core3Sample.Controllers
{

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        public BaseApiController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
                            IConfiguration configuration, IHttpContextAccessor httpContextAccessor = null)
        {
            // Instantiate the required classes through DI
            DbContext = context;
            UserManager = userManager;
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
            // Instantiate a single JsonSerializerSettings object
            // that can be reused multiple times.
            JsonSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
        }

        protected IHttpContextAccessor HttpContextAccessor
        {
            get;
            private set;
        }

        protected ApplicationDbContext DbContext
        {
            get;
            private set;
        }

        protected UserManager<ApplicationUser> UserManager
        {
            get;
            private set;
        }

        protected IConfiguration Configuration
        {
            get;
            private set;
        }

        protected JsonSerializerSettings JsonSettings
        {
            get;
            private set;
        }
        
    }
}

