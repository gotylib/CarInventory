using CarInventory.CarInventory.Dal;
using CarInventory.CarInventory.Models;

namespace CarInventory.CarInventory.Bll
{
    public interface ITokenService
    {
        string GenerateAccessToken(ApplicationUser user);
        RefreshTokenModel GenerateRefreshToken();
        bool ValidateRefreshToken(string refreshToken);
    }
}
