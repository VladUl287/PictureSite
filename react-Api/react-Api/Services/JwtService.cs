using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace react_Api.Services
{
    public class JwtService
    {
        public static string Generate(int id, string email, string role, string key, DateTime expires)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var credentialsAccess = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[]
            {
                new Claim("id", id.ToString()),
                new Claim("email", email),
                new Claim("role", role)
            };

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: credentialsAccess);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public static bool ValidateToken(string token, string key)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = symmetricSecurityKey
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}