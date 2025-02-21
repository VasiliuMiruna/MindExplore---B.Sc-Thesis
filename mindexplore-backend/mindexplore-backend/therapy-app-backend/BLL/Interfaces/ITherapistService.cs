using therapy_app_backend.BLL.Models;

namespace therapy_app_backend.BLL.Interfaces
{
    public interface ITherapistService
    {
        Task CreateTherapist(TherapistRegisterModel registerModel);
       
        Task<bool> UpdateById(int id, TherapistModel therapist);
        
        Task<TherapistModel> GetById(int id);
        Task<List<TherapistModel>> GetAll();
        Task<bool> DeleteById(int id);
        //Task UpdateRole(string userEmail, string newRole);
        Task GiveRating(int therapistId, int rating);
        Task<List<TherapistModel>> GetTherapistsBySpecialties(List<int> specialtiesIds);
        Task<List<TherapistModel>> GetTherapistsByAscendingPrice();
        Task<List<TherapistModel>> GetTherapistsBySpecialtiesAndAscendingPrice(List<int> specialtiesIds);
        Task<List<TherapistModel>> GetTherapistsByDescendingPrice();
        Task<List<TherapistModel>> GetTherapistsBySpecialtiesAndDescendingPrice(List<int> specialtiesIds);
        Task<List<PatientModel>> GetPatients(int therapistId);
        Task<List<string>> GetSpecialtiesOfTherapist(int therapistId);


    }
}
