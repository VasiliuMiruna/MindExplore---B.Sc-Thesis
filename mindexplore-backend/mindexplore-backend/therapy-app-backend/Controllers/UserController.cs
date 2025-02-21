using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{userId}/details")]
        public async Task<UserRetrieve> GetUserDetailsById([FromRoute]string userId)
        {
            var result = await _userRepository.GetUserDetails(userId); 
            return result;
        }
        
        [HttpGet("{userId}/{userRole}")]
        public int GetShortIdByUserId([FromRoute]string userId, [FromRoute] string userRole)
        {
            
              var shortId =   _userRepository.GetShortIdByUserId(userId, userRole);
              return shortId;
        }
        //httpget-retrieve name by userid
        //httpget long id by short id
    }
}
