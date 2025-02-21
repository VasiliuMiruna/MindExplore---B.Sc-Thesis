using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using therapy_app_backend.BLL.Interfaces;
using therapy_app_backend.BLL.Models;

namespace therapy_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;
        public SpecialtyController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        [HttpPost("specialty")]
        public async Task<IActionResult> CreateSpecialty([FromBody]string name)
        {
            try
            {
                var result = await _specialtyService.CreateSpecialty(name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("specialties")]
        public async Task<List<SpecialtyModel>> GetSpecialties()
        {
            var specialties = await _specialtyService.GetSpecialties();
            specialties.Remove(specialties[0]);
            specialties.Remove(specialties[0]);

            return specialties;
        }
    }
}
