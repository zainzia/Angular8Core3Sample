using System;
using System.Threading.Tasks;

using Angular8Core3Sample.Data;
using Angular8Core3Sample.Models.Identity;


using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

using Mapster;
using Angular8Core3Sample.Services;

namespace Angular8Core3Sample.Controllers.Identity
{
    [Route("API/Registration")]
    public class RegistrationController : BaseApiController
    {

        RegistrationService _registrationService { get; set; }

        #region Constructor
        public RegistrationController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager, IConfiguration configuration, RegistrationService registrationService) 
            : base(context, userManager, configuration) {

            _registrationService = registrationService;
        
        }
        #endregion

        #region RESTful Conventions

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationRequest registrationRequest)
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (registrationRequest == null)
            {
                return new StatusCodeResult(500);
            }

            var result = await _registrationService.RegisterNewUser(registrationRequest).ConfigureAwait(false);

            // return the newly-created User to the client.
            return new JsonResult(result, JsonSettings);
        }
        #endregion

    }
}
