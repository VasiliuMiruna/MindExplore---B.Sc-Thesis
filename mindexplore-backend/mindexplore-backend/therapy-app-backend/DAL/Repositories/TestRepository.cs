using Microsoft.EntityFrameworkCore;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.DAL.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly AppDbContext _db;

        public TestRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddTest(Test test)
        {
           var testAdded = await _db.Tests.FirstOrDefaultAsync(t => t.PatientId == test.PatientId);
           if(testAdded != null)
           {
                 _db.Tests.Remove(testAdded);
               
           }
           await _db.Tests.AddAsync(test);
           await _db.SaveChangesAsync();
        }

        public async Task<Test> GetTestByPatientId(int id)
        {
            var test = await _db.Tests.FirstOrDefaultAsync(t => t.PatientId == id);
            return test;
        }
    }
}
