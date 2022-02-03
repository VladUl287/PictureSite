using Microsoft.AspNetCore.Mvc;
using react_Api.Models;
using react_Api.Services;
using System;
using System.Threading.Tasks;

namespace react_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;

        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var result = await authService.Login(login);

            return result.Match<IActionResult>(
                success =>
                {
                    Response.Cookies.Append("token", success.RefrechToken, new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        MaxAge = TimeSpan.FromDays(15)
                    });

                    return Ok(success);
                },
                faild => BadRequest(Errors.NotCorrectEmailOrPassword)
            );
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel register)
        {
            var result = await authService.Register(register);

            return result.Match<IActionResult>(
                user => CreatedAtAction("Register", new { user.Id }),
                exists => BadRequest(Errors.UserAlreadyExists)
            );
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["token"];

            if (refreshToken is not null)
            {
                await authService.Logout(refreshToken);

                Response.Cookies.Delete("token");
            }

            return NoContent();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var cookieToken = Request.Cookies["token"];

            var result = await authService.Refresh(cookieToken);

            return result.Match<IActionResult>(
                success => Ok(new { token = success.AccessToken }),
                faild =>
                {
                    Response.Cookies.Delete("token");
                    return Unauthorized();
                }
           );
        }
    }
}