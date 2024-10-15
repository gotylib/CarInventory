using CarInventory.CarInventory.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarInventory.CarInventory.Api
{
    public interface IAuthService
    {
        Task<IActionResult> Register(RegisterModel model);
        Task<IActionResult> Login(LoginModel model);
        Task<IActionResult> RefreshToken(RefreshTokenModel model);
    }

}
