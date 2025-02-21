using therapy_app_backend.DAL.Entities;

namespace therapy_app_backend.DAL.Interfaces
{
    public interface ITestRepository
    {
        Task AddTest(Test test);
        Task<Test> GetTestByPatientId(int id);
    }
}
