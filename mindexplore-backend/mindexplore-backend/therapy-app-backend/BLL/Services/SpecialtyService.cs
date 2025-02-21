using therapy_app_backend.BLL.Interfaces;
using therapy_app_backend.BLL.Models;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.BLL.Services
{
    public class SpecialtyService: ISpecialtyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpecialtyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> CreateSpecialty(string name)
        {
            
            var specialty = _unitOfWork.Specialties.GetByName(name);
            if (specialty.Result == null)
            {
                var newSpecialty = new Specialty();
                newSpecialty.Name = name;

                await _unitOfWork.Specialties.AddSpecialty(newSpecialty);
                return $"Specialty {name} created";
            }
            else
            {
                throw new Exception("The specialty already exists");

            }
        }
        public async Task<List<SpecialtyModel>> GetSpecialties()
        {
            var specialties = await _unitOfWork.Specialties.GetAllSpecialties();
            var specialtyModels = new List<SpecialtyModel>();
            foreach (var specialty in specialties)
            {
                var specialtyModel = new SpecialtyModel {
                    Name = specialty.Name,
                    Id = specialty.Id
                };
                specialtyModels.Add(specialtyModel);
            }
            return specialtyModels;
        }

     }
}
