using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BobsCorn.Domain.Services;
using Microsoft.IdentityModel.Tokens;

namespace BobsCorn.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey = Environment.GetEnvironmentVariable("ENCRYPTION_KEY") ?? "Secret key is missing";

        public string GenerateToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
