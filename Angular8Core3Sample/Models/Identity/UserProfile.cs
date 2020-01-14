using Angular8Core3Sample.Models.DataModels.Catalog;

namespace Angular8Core3Sample.Models.Identity
{
    public class UserProfile
    {

        public int UserProfileID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public Language Language { get; set; }

        public Address Address { get; set; }

        public ApplicationUser User { get; set; }
    }
}