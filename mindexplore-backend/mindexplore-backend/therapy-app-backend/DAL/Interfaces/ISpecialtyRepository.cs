using therapy_app_backend.DAL.Entities;

namespace therapy_app_backend.DAL.Interfaces
{
    public interface ISpecialtyRepository
    {
        Task AddSpecialty(Specialty specialty);
        Task DeleteSpecialty(Specialty specialty);
        Task<Specialty> GetById(int Id);
        Task<Specialty> GetByName(string name);
        Task<List<Specialty>> GetAllSpecialties();
        Task AddTherapistSpecialty(int therapistId, int specialtyId);

    }
}
