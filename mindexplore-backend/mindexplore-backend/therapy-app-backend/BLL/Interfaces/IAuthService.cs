using therapy_app_backend.BLL.Models;

namespace therapy_app_backend.BLL.Interfaces
{
    public interface IAuthService
    {
        Task Register(RegisterModel registerModel);
        Task<LoginResult> Login(LoginModel loginModel);
        Task<LoginResult> Refresh(string refreshToken);
    }
}
