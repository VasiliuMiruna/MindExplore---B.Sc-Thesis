using therapy_app_backend.BLL.Models;

namespace therapy_app_backend.BLL.Interfaces
{
    public interface ISpecialtyService
    {
        Task<string> CreateSpecialty(string name);
        Task<List<SpecialtyModel>> GetSpecialties();
    }
}
