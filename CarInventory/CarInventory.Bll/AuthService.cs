using CarInventory.CarInventory.Bll;
using CarInventory.CarInventory.Dal.BaseObjects;
using CarInventory.CarInventory.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarInventory.CarInventory.Api
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<IActionResult> Register(RegisterModel model)
        {

            var user = new User { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                if (model.SecretCode != null)
                {
                    if (model.SecretCode == "Admin")
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    else if (model.SecretCode == "Manager")
                    {
                        await _userManager.AddToRoleAsync(user, "Manager");
                    }
                }

                return new OkResult();
            }

            return new BadRequestObjectResult(result.Errors);
        }

        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return new UnauthorizedResult();
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken.Token; // Обновление токена в базе данных
            await _userManager.UpdateAsync(user);

            return new OkObjectResult(new { AccessToken = accessToken, RefreshToken = refreshToken.Token });
        }

        public async Task<IActionResult> RefreshToken(RefreshTokenModel model)
        {
            if (string.IsNullOrEmpty(model.Token))
            {
                return new BadRequestObjectResult("Refresh token is required.");
            }

            var user = await _userManager.Users.FirstOrDefaultAsync( u => u.RefreshToken == model.Token);

            if (user == null || user.RefreshTokenExpiration < DateTime.UtcNow)
            {
                return new UnauthorizedResult();
            }

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken.Token;
            user.RefreshTokenExpiration = newRefreshToken.Expiration;
            await _userManager.UpdateAsync(user);

            return new OkObjectResult(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken.Token });
        }
    }

}
