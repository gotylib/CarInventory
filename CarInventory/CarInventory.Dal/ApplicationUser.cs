using Microsoft.AspNetCore.Identity;
namespace CarInventory.CarInventory.Dal
{

    public class ApplicationUser : IdentityUser
    {
        public string? Role { get; set; }
        public string? Password { get; set; }
        public string? RefreshToken { get; set; }
    }


}
