using Microsoft.AspNetCore.Identity;

namespace WebApplication7.Models
{
    public class UserIdentitycs: IdentityUser
    {
        public string Name { get; set; }
    }
}
