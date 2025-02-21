using therapy_app_backend.BLL.Models;

namespace therapy_app_backend.BLL.Interfaces
{
    public interface IPatientService
    {
        Task<List<PatientModel>> GetAll();
        Task<PatientModel> GetById(int id);
        Task<bool> UpdateById(int id, PatientModel patient);
        Task<bool> DeleteById(int id);
        Task<List<TherapistModel>> GetPatientsTherapists(int patientId);


    }
}
