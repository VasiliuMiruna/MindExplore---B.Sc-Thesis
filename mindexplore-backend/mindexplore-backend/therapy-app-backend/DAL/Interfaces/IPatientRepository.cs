using therapy_app_backend.DAL.Entities;

namespace therapy_app_backend.DAL.Interfaces
{
    public interface IPatientRepository
    {
        Task Create(Patient patient);
        Task<List<Patient>> GetAll();
        Task<Patient> GetById(int id);
        Task UpdatePatient(Patient patient);
        Task DeletePatient(Patient patient);
        Task<List<Therapist>> GetAllTherapists(int patientId);


    }
}
