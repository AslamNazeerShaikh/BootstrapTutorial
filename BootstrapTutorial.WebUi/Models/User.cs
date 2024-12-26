using Microsoft.AspNetCore.Identity;

namespace BootstrapTutorial.WebUi.Models
{
    public class User : IdentityUser
    {
        // Basic User Details
        public string    FirstName        { get; set; } = string.Empty;
        public string    LastName         { get; set; } = string.Empty;
        public DateTime  BirthDate        { get; set; }

        // Type of user: Admin, Internal Employee, or External User
        public string    UserType         { get; set; } = "ExternalUser"; // Default is External User

        // Audit fields
        public DateTime  CreatedAt        { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt        { get; set; }

        // Optional fields for employee-specific data
        public string?   Department       { get; set; } // Internal Employees
        public string?   OrganizationName { get; set; } // Internal Employees

        // Optional fields for external user-specific data
        public string?   CompanyName      { get; set; } // External Users
    }
}
