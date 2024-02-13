using Microsoft.AspNetCore.Identity;

namespace TheFinalProject.Entities
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
