using Microsoft.AspNetCore.Identity;

namespace Auth_jwt.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public string? Gender { get; set; }
        public string? ProfileImagePath { get; set; }
    }
}
