using therapy_app_backend.DAL.Interfaces;
using therapy_app_backend.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace therapy_app_backend.DAL.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _db;

        public PatientRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task Create(Patient patient)
        {
            await _db.Patients.AddAsync(patient);
            await _db.SaveChangesAsync();
        }
        public async Task<List<Patient>> GetAll()
        {
            return _db.Patients.ToList();

        }

        public async Task<Patient> GetById(int id)
        {
            return _db.Patients.FirstOrDefault(p => p.PatientId == id);
        }

        public async Task UpdatePatient(Patient patient)
        {
            _db.Patients.Update(patient);
            await _db.SaveChangesAsync();
        }

        public async Task DeletePatient(Patient patient)
        {
            _db.Patients.Remove(patient);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Therapist>> GetAllTherapists(int patientId)
        {
            var therapists = await _db.Appointments
                .Where(a => a.PatientId == patientId)
                .Select(a => a.Therapist)
                .Distinct()
                .ToListAsync();
            return therapists;
        }

    }
}
