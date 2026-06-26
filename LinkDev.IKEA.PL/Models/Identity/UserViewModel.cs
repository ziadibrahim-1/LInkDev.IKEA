using System.Collections.Generic;

namespace LinkDev.IKEA.PL.Models.Identity
{
    public class UserViewModel
    {
        public string Id { get; set; }= string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
