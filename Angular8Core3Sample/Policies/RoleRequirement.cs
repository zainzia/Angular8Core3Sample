using Microsoft.AspNetCore.Authorization;


namespace Angular8Core3Sample.Policies
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public  string roleName { get; private set; }

        public RoleRequirement(string RoleName)
        {
            roleName = RoleName;
        }
    }
}
