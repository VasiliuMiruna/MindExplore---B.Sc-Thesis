using System.Security.Claims;
using therapy_app_backend.DAL.Entities;

namespace therapy_app_backend.BLL.Interfaces
{
    public interface ITokenHelper
    {
        Task<String> CreateAccessToken(AppUser _User);
        string CreateRefreshToken(AppUser _User);
        ClaimsPrincipal GetPrincipalFromRefreshToken(string _Token);
    }
}
