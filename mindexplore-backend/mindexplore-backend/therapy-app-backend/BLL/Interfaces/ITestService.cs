using therapy_app_backend.DAL.Entities;

namespace therapy_app_backend.BLL.Interfaces
{
    public interface ITestService
    {
        Task<Test> GetTestResultByPatientId(int id);
        Task<List<Therapist>> GetRecommendedTherapists(Dictionary<string, string> testF, int patientId);
        Task<List<(Therapist, double)>> ShowTherapistsScores(Dictionary<string, string> testF);

    }
}
