using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using react_Api.Database;
using react_Api.Database.Models;
using react_Api.Models;
using react_Api.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace react_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext dbContext;
        private readonly string passwordKey = string.Empty;
        private readonly string accessTokenKey = string.Empty;
        private readonly string refreshTokenKey = string.Empty;

        public AuthController(
            DatabaseContext databaseContext,
            IConfiguration configuration)
        {
            this.dbContext = databaseContext;
            passwordKey = configuration.GetValue<string>("Secrets:PasswordSecret");
            accessTokenKey = configuration.GetValue<string>("Secrets:JwtAccessSecret");
            refreshTokenKey = configuration.GetValue<string>("Secrets:JwtRefreshSecret");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
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

            var hashPassword = HashService.Hash(login.Password, passwordKey);

            if (user is not null && user.Password == hashPassword)
            {
                GenerateTokens(user.Id, user.Email, user.Role, out string accessToken, out string refreshToken);

                await dbContext.Tokens.AddAsync(new Token
                {
                    UserId = user.Id,
                    RefreshToken = refreshToken
                });
                await dbContext.SaveChangesAsync();

                Response.Cookies.Append("token", refreshToken, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    MaxAge = TimeSpan.FromDays(30)
                });

                return Ok(new { token = accessToken });
            }

            return BadRequest(new { error = "Неверный email или пароль." });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel register)
        {
            var exists = await dbContext.Users
                .AnyAsync(e => e.Email == register.Email);

            if (exists)
            {
                return BadRequest(new { error = "Пользователь с таким email уже существует." });
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

            return CreatedAtAction("Register", user);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["token"];

            if (refreshToken is not null)
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"DELETE FROM [Tokens] WHERE [RefreshToken] LIKE {refreshToken}");

                Response.Cookies.Delete("token");
            }

            return NoContent();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var cookieToken = Request.Cookies["token"];

            if (cookieToken is null)
            {
                return Unauthorized();
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
                .FirstOrDefaultAsync(x => x.RefreshToken == cookieToken);

            if (dbToken is null)
            {
                Response.Cookies.Delete("token");

                return Unauthorized();
            }

            var valid = JwtService.ValidateToken(dbToken.RefreshToken, refreshTokenKey);

            if (!valid)
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                   $"DELETE FROM [Tokens] WHERE [RefreshToken] LIKE {dbToken.RefreshToken}");

                Response.Cookies.Delete("token");

                return Unauthorized();
            }

            GenerateTokens(dbToken.UserId, dbToken.Email, dbToken.RoleName, out string accessToken, out string refreshToken);

            await dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE [Tokens] SET [RefreshToken] = {dbToken.RefreshToken} WHERE [Id] = {dbToken.UserId}");

            return Ok(new { token = accessToken });
        }

        private void GenerateTokens(int id, string email, string role, out string accessToken, out string refreshToken)
        {
            accessToken = JwtService.Generate(
                            id,
                            email,
                            role,
                            accessTokenKey,
                            DateTime.Now.AddMinutes(15));
            refreshToken = JwtService.Generate(
                            id,
                            email,
                            role,
                            refreshTokenKey,
                            DateTime.Now.AddDays(15));
        }
    }
}