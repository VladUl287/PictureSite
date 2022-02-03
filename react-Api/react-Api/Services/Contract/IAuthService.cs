using OneOf;
using react_Api.Database.Models;
using react_Api.Models;
using react_Api.ModelsDto;
using react_Api.ViewModels;
using System.Threading.Tasks;

namespace react_Api.Services.Contract
{
    public interface IAuthService
    {
        Task<OneOf<LoginSuccess, NotValidToken>> Refresh(string token);

        Task<OneOf<LoginSuccess, NotCorrectData>> Login(LoginModel login);

        Task<OneOf<User, EmailAlreadyExists>> Register(RegisterModel register);

        Task Logout(string token);
    }
}