using Microsoft.AspNetCore.Identity;
namespace CarInventory.CarInventory.Dal.BaseObjects
{

    public class User : IdentityUser
    {
        public string? Password { get; set; }
        public string? RefreshToken { get; set; }

    }


}
