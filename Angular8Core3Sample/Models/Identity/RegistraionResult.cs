
using System.Collections.Generic;

namespace Angular8Core3Sample.Models.Identity
{
    public enum RegistraionResultEnum
    {
        UsernameAlreadyExists,
        EmailAlreadyRegistered,
        Succeeded,
        Failed
    }

    public class RegistraionResult
    {
        public RegistraionResultEnum Result { get; set; }

        public UserProfile UserProfile { get; set; }

        public List<string> Errors { get; set; }
    }
}
