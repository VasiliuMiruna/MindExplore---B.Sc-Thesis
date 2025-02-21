using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using therapy_app_backend.BLL.Interfaces;
using therapy_app_backend.BLL.Models;
using therapy_app_backend.DAL.Entities;

namespace therapy_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;
        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpPost("{patientId}")]
        public async Task<List<Therapist>> GetRecommendations([FromBody] Dictionary<string, string> test, [FromRoute] int patientId)
        {
            var recommendedTherapists = await _testService.GetRecommendedTherapists(test, patientId);
            return recommendedTherapists;
        }
        [HttpGet("{patientId}")]
        public async Task<Test> GetPatientTest([FromRoute] int patientId)   
        {
            var test = await _testService.GetTestResultByPatientId(patientId);
             return test;
        }
        [HttpGet("{patientId}/recommendations")]
        public async Task<List<Therapist>> GetRecommendationsTherapists(int patientId)
        {
            var test = await _testService.GetTestResultByPatientId(patientId);
            var recommendedTherapists = await _testService.GetRecommendedTherapists(test.Questions, patientId);
            return recommendedTherapists;

        }
        //Task<List<(Therapist, double)>> ShowTherapistsScores(Dictionary<string, string> testF)
        [HttpGet]
        public async Task<List<(Therapist, double)>> GetDoubleTest([FromBody] Dictionary<string, string> test)
        {
            var list = await _testService.ShowTherapistsScores(test);
            return list;
        }
    }  
}
