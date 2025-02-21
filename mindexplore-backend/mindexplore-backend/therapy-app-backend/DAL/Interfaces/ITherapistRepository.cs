using therapy_app_backend.DAL.Entities;

namespace therapy_app_backend.DAL.Interfaces
{
    public interface ITherapistRepository
    {
        Task Create(Therapist therapist);
        Task Update(Therapist therapist);
        Task<Therapist> GetById(int id);
        Task<List<Therapist>> GetAll();
        Task DeleteTherapist(Therapist therapist);
        Task<List<Patient>> GetPatientsByTherapistId(int therapistId);
        Task<List<int>> GetSpecialtiesByTherapistId(int therapistId);
        Task UpdateSpecialtiesOfTherapist(int therapistId, List<int> newSpecialties);
        Task<List<Therapist>> FilterBySpecialties(List<int> specialtyIds);
        Task<List<Therapist>> FilterByAscendingPrice();
        Task<List<Therapist>> FilterBySpecialtiesAndAscendingPrice(List<int> specialtyIds);
        Task<List<Therapist>> FilterByDescendingPrice();
        Task<List<Therapist>> FilterBySpecialtiesAndDescendingPrice(List<int> specialtyIds);
        Task<List<Specialty>> GetSpecialties(int therapistId);
        Task AddPatientToTherapist(int patientId, int therapistId);


    }
}
