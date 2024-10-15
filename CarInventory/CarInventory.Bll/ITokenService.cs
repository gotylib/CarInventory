using CarInventory.CarInventory.Dal.BaseObjects;
using CarInventory.CarInventory.Models;

namespace CarInventory.CarInventory.Bll
{
    public interface ITokenService
    {
        
        string GenerateAccessToken(User user);
        RefreshTokenModel GenerateRefreshToken();
    }
}
