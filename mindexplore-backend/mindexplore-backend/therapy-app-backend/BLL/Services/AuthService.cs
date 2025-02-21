using Microsoft.AspNetCore.Identity;
using therapy_app_backend.DAL.Interfaces;
using therapy_app_backend.BLL.Models;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.BLL.Interfaces;

namespace therapy_app_backend.BLL.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenHelper _tokenHelper;
        private readonly IUnitOfWork _unitOfWork;


        public AuthService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenHelper tokenHelper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHelper = tokenHelper;
            _unitOfWork = unitOfWork;

        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
                return new LoginResult
                {
                    Success = false
                };
            else
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
                if (result.Succeeded)
                {
                    var token = await _tokenHelper.CreateAccessToken(user);
                    var refreshToken = _tokenHelper.CreateRefreshToken(user);

                    return new LoginResult
                    {
                        Success = true,
                        AccessToken = token,
                        RefreshToken = refreshToken

                    };
                }
                else
                    return new LoginResult
                    {
                        Success = false
                    };
            }


        }
        public async Task Register(RegisterModel registerModel)
        {
            var user = new AppUser
            {
                Email = registerModel.Email,
                UserName = registerModel.Email

            };
            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Patient");
                
                var patient = new Patient
                {
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    User = user,
                    UserId = user.Id
                };

                await _unitOfWork.Patients.Create(patient);
                await _unitOfWork.SaveChangesAsync();

            }
            else
            {
                throw new Exception(String.Join(",", result.Errors.Select(x => x.Code)));
            }

        }
        public async Task<LoginResult> Refresh(string refreshToken)
        {
            var claimsPrincipal = _tokenHelper.GetPrincipalFromRefreshToken(refreshToken);
            var userId = claimsPrincipal.Identity.Name;
            var user = await _userManager.FindByNameAsync(userId);
            if (user == null)
                return new LoginResult
                {
                    Success = false,
                };
            else
            {
                var token = await _tokenHelper.CreateAccessToken(user);
                refreshToken = _tokenHelper.CreateRefreshToken(user);

                return new LoginResult
                {
                    Success = true,
                    AccessToken = token,
                    RefreshToken = refreshToken

                };
            }

        }
    }
}
