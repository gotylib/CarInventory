using CarInventory.CarInventory.Dal.BaseObjects;
using Microsoft.AspNetCore.Identity;

namespace CarInventory.CarInventory.Bll
{
    public interface IUserService
    {
        Task<List<User>> GetUsersAsync();
        Task<IdentityResult> CreateUserAsync(User user);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IdentityResult> DeleteUserAsync(User user);
    }

}
