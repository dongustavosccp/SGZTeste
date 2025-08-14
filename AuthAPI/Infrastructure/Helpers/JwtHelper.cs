using AuthAPI.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Utils.Helpers
{
    public class JwtHelper
    {
        private readonly string _secretKey;

        public JwtHelper(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:SecretKey"] ?? "";
        }

        public string GenerateToken(User user)
        {
            if (string.IsNullOrWhiteSpace(_secretKey))
            {
                throw new InvalidOperationException("JWT secret key is not configured.");
            }

            JwtSecurityTokenHandler tokenHandler = new();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString() ?? "User")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(
                subject: tokenDescriptor.Subject,
                expires: tokenDescriptor.Expires,
                signingCredentials: tokenDescriptor.SigningCredentials
            );
            return tokenHandler.WriteToken(token);
        }
    }
}