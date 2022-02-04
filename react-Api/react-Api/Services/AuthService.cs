using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OneOf;
using react_Api.Database;
using react_Api.Database.Models;
using react_Api.Models;
using react_Api.ModelsDto;
using react_Api.Services.Contract;
using react_Api.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace react_Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseContext dbContext;
        private readonly IConfiguration configuration;

        public AuthService(DatabaseContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        public async Task<OneOf<LoginSuccess, NotCorrectData>> Login(LoginModel login)
        {
            var user = await dbContext.Users
                .AsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.Email,
                    x.Password,
                    Role = x.Role.Name,
                })
                .FirstOrDefaultAsync(x => x.Email == login.Email);

            if (user is null)
            {
                return new NotCorrectData();
            }

            var key = configuration.GetValue<string>("Secrets:PasswordSecret");
            var hashPassword = HashService.Hash(login.Password, key);

            if (user.Password != hashPassword)
            {
                return new NotCorrectData();
            }

            GenerateTokens(user.Id, user.Email, user.Role, out string accessToken, out string refreshToken);

            await dbContext.Tokens.AddAsync(new Token
            {
                UserId = user.Id,
                RefreshToken = refreshToken
            });
            await dbContext.SaveChangesAsync();

            return new LoginSuccess
            {
                Id = user.Id,
                Email = user.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<OneOf<User, EmailAlreadyExists>> Register(RegisterModel register)
        {
            var exists = await dbContext.Users
               .AnyAsync(e => e.Email == register.Email);

            if (exists)
            {
                return new EmailAlreadyExists();
            }

            var user = new User
            {
                Email = register.Email,
                Login = register.Login,
                Password = register.Password,
                RoleId = 2
            };

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return user;
        }

        public async Task Logout(string token)
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"DELETE FROM [Tokens] WHERE [RefreshToken] LIKE {token}");
        }

        public async Task<OneOf<LoginSuccess, NotValidToken>> Refresh(string token)
        {
            if (token is null)
            {
                return new NotValidToken();
            }

            var dbToken = await dbContext.Tokens
               .AsNoTracking()
               .Select(x => new
               {
                   x.UserId,
                   x.User.Email,
                   x.RefreshToken,
                   RoleName = x.User.Role.Name
               })
               .FirstOrDefaultAsync(x => x.RefreshToken == token);

            var valid = JwtService.ValidateToken(dbToken.RefreshToken, configuration.GetValue<string>("Secrets:JwtRefreshSecret"));

            if (!valid)
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                   $"DELETE FROM [Tokens] WHERE [RefreshToken] LIKE {dbToken.RefreshToken}");

                return null;
            }

            GenerateTokens(dbToken.UserId, dbToken.Email, dbToken.RoleName, out string accessToken, out string refreshToken);

            await dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE [Tokens] SET [RefreshToken] = {dbToken.RefreshToken} WHERE [Id] = {dbToken.UserId}");

            return new LoginSuccess { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        private void GenerateTokens(int id, string email, string role, out string accessToken, out string refreshToken)
        {
            accessToken = JwtService.Generate(
                            id,
                            email,
                            role,
                            configuration.GetValue<string>("Secrets:JwtAccessSecret"),
                            DateTime.Now.AddMinutes(15));
            refreshToken = JwtService.Generate(
                            id,
                            email,
                            role,
                            configuration.GetValue<string>("Secrets:JwtRefreshSecret"),
                            DateTime.Now.AddDays(15));
        }
    }
}