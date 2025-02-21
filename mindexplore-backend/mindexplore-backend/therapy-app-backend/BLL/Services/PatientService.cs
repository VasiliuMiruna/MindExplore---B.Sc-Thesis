using therapy_app_backend.BLL.Interfaces;
using therapy_app_backend.BLL.Models;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.BLL.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<PatientModel>> GetAll()
        {
            var patientsDb = await _unitOfWork.Patients.GetAll();
            var list = new List<PatientModel>();
            foreach (var patient in patientsDb)
            {
                var patientModel = new PatientModel
                {
                    Id = patient.PatientId,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    Age = patient.Age,
                    City = patient.City,

                };
                list.Add(patientModel);

            }

            return list;
        }
        public async Task<PatientModel> GetById(int id)
        {
            var patient = await (_unitOfWork.Patients.GetById(id));
            if (patient == null)
            {
                return null;
            }
            var patientModel = new PatientModel
            {
                Id = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Age = patient.Age,
                City= patient.City,

            };
            return patientModel;
        }
        public async Task<bool> UpdateById(int id, PatientModel patient)
        {
            var patientDb = await _unitOfWork.Patients.GetById(id);
            if (patientDb != null)
            {
                patientDb.PatientId = id;
                patientDb.FirstName = patient.FirstName;
                patientDb.LastName = patient.LastName;
                patientDb.Age = patient.Age;
                patientDb.City = patient.City;
                await _unitOfWork.Patients.UpdatePatient(patientDb);
                return true;
            }
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var patientDb = await _unitOfWork.Patients.GetById(id);
            if (patientDb != null)
            {
                await _unitOfWork.Patients.DeletePatient(patientDb);
                return true;
            }
            else return false;

        }

        public async Task<List<TherapistModel>> GetPatientsTherapists(int patientId)
        {
            var therapists = await _unitOfWork.Patients.GetAllTherapists(patientId);
            var therapistList = new List<TherapistModel>();
            foreach (var therapist in therapists)
            {
                var therapistModel = new TherapistModel
                {
                    Id = therapist.TherapistId,
                    FirstName = therapist.FirstName,
                    LastName = therapist.LastName,
                    LicenseNumber = therapist.LicenseNumber,
                    LicenseState = therapist.LicenseState,
                    Age = therapist.Age,
                    Rating = therapist.Rating,
                    Price = therapist.Price,
                    NumberOfPatinents = therapist.NumberOfPatinents,
                    City = therapist.City,
                    IsAvailable = therapist.IsAvailable,
                    NoOfRatings = therapist.NoOfRatings,


                };
                therapistList.Add(therapistModel);
            }
            return therapistList;
        }

       
            /*var patients = await _unitOfWork.Therapists.GetPatientsByTherapistId(therapistId);
            var patientList = new List<PatientModel>();
            foreach (var patient in patients)
            {
                var patientModel = new PatientModel
                {
                    Id = patient.PatientId,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    Age = patient.Age,
                    City = patient.City,
                };
                patientList.Add(patientModel);
            }
            return patientList;*/
        
    }
}
