using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System.Linq;
using Angular8Core3Sample.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace Angular8Core3Sample.Policies
{
    public class AdministratorAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {

        UserManager<ApplicationUser> _userManager;


        public AdministratorAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (context.User == null)
            {
                throw new ArgumentException();
            }

            var user = await _userManager.GetUserAsync(context.User);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles != null)
                {

                    var hasRole = roles.FirstOrDefault(x => x == requirement.roleName);

                    //var hasRole =  (await _userManager.GetRolesAsync(await _userManager.GetUserAsync(context.User))).FirstOrDefault(r => r == requirement.roleName);

                    // Administrators can do anything.
                    if (!string.IsNullOrEmpty(hasRole))
                    {
                        context.Succeed(requirement);
                    }
                }
            }
        }

    }
}

