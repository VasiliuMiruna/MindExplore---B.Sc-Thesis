using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using therapy_app_backend.BLL.Interfaces;
using therapy_app_backend.BLL.Models;

namespace therapy_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            try
            {

                Console.WriteLine(registerModel);
                await _authService.Register(registerModel);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var result = await _authService.Login(loginModel);
            return result.Success ? Ok(result) : BadRequest("Failed to login");
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var result = await _authService.Refresh(refreshToken);
            return result.Success ? Ok(result) : BadRequest("Failed to refresh Token");
        }


    }
}

