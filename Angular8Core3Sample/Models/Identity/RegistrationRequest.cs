
using Newtonsoft.Json;

namespace Angular8Core3Sample.Models.Identity
{

    [JsonObject(MemberSerialization.OptOut)]
    public class RegistrationRequest
    {

        #region Properties
        public UserProfile UserProfile { get; set; }
        
        public string Password { get; set; }
        
        public string ClientId { get; set; }
        #endregion
    }

}

