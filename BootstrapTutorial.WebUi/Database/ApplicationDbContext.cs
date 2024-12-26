using BootstrapTutorial.WebUi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BootstrapTutorial.WebUi.Database
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "InternalEmployee", NormalizedName = "INTERNALEMPLOYEE" },
                new IdentityRole { Id = "3", Name = "ExternalUser", NormalizedName = "EXTERNALUSER" }
            );

            // Seed Admin User
            var adminUser = new User
            {
                Id                 = Guid.NewGuid().ToString(),
                UserName           = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                Email              = "admin@example.com",
                NormalizedEmail    = "ADMIN@EXAMPLE.COM",
                EmailConfirmed     = true,
                UserType           = "Admin",
                FirstName          = "System",
                LastName           = "Administrator",
                CreatedAt          = DateTime.UtcNow
            };

            var passwordHasher = new PasswordHasher<User>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123");

            builder.Entity<User>().HasData(adminUser);

            // Assign Admin Role
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = adminUser.Id, RoleId = "1" }
            );

            // Seed Claims
            builder.Entity<IdentityRoleClaim<string>>().HasData(
                new IdentityRoleClaim<string> { Id = 1, RoleId = "1", ClaimType = "Permission", ClaimValue = "ManageUsers" }
            );
        }
    }
}
