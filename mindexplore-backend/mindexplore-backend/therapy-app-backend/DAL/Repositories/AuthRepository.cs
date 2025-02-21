using Microsoft.AspNetCore.Identity;
using therapy_app_backend.DAL.Interfaces;
using therapy_app_backend.DAL.Entities;

namespace therapy_app_backend.DAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public AuthRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
    }
}
