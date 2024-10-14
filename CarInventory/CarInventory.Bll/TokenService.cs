using CarInventory.CarInventory.Dal;
using CarInventory.CarInventory.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CarInventory.CarInventory.Bll
{
    public class TokenService : ITokenService
    {
        private const int RefreshTokenExpirationDays = 1; 

        public string GenerateAccessToken(ApplicationUser user)
        {
            var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = AuthOptions.GetSymmetricSecurityKey();
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshTokenModel GenerateRefreshToken()
        {
            return new RefreshTokenModel
            {
                Token = Guid.NewGuid().ToString(),
                Expiration = DateTime.UtcNow.AddDays(RefreshTokenExpirationDays)
            };
        }

        public bool ValidateRefreshToken(string refreshToken)
        {
            // Логика валидации refresh token
            // Например, проверка на наличие и истечение срока действия
            return true; // Замените на реальную логику
        }


    }

}
