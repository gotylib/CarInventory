using Microsoft.AspNetCore.Identity;
namespace CarInventory.CarInventory.Dal
{

    public class ApplicationUser : IdentityUser
    {
      public string? Password { get; set; }
    }


}
