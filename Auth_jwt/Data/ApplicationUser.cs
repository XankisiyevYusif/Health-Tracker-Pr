using Microsoft.AspNetCore.Identity;

namespace Auth_jwt.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
