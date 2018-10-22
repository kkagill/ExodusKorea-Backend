using Microsoft.AspNetCore.Identity;

namespace ExodusKorea.Model.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }
    }
}