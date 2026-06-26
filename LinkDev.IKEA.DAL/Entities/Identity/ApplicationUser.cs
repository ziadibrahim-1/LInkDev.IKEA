using Microsoft.AspNetCore.Identity;

namespace LinkDev.IKEA.DAL.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public bool IsAgree { get; set; } 
    }
}
