using Auth_jwt.Models;
using Auth_jwt.Models.WaterIntake;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth_jwt.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<StepData> StepData { get; set; }
        public DbSet<WaterIntake> WaterIntakes { get; set; }
        public DbSet<CalorieData> CalorieData { get; set; } 
    }
}
