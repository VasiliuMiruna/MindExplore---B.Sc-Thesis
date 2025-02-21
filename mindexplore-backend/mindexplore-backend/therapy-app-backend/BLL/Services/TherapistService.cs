using Microsoft.AspNetCore.Identity;
using therapy_app_backend.BLL.Interfaces;
using therapy_app_backend.BLL.Models;
using therapy_app_backend.DAL.Interfaces;
using therapy_app_backend.DAL.Entities;
using System.Net.Mail;

namespace therapy_app_backend.BLL.Services
{
    public class TherapistService : ITherapistService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenHelper _tokenHelper;
        private readonly IUnitOfWork _unitOfWork;



        public TherapistService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenHelper tokenHelper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHelper = tokenHelper;
            _unitOfWork = unitOfWork;


        }
        public async Task CreateTherapist(TherapistRegisterModel registerModel)
        {
            var user = new AppUser
            {
                Email = registerModel.Email,
                UserName = registerModel.Email

            };
            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                
                await _userManager.AddToRoleAsync(user, "Therapist");

                var therapist = new Therapist
                {
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    LicenseNumber = registerModel.LicenseNumber,
                    IsAdded = false,
                    //LicenseState = registerModel.LicenseState,
                    User = user,
                    UserId = user.Id
                };

                await _unitOfWork.Therapists.Create(therapist);
                await _unitOfWork.SaveChangesAsync();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.Credentials = new System.Net.NetworkCredential("mindexploretest@gmail.com", "iqpnjnzodjoqafqw");
                // smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                MailMessage mail = new MailMessage();
                mail.Subject = "O nouă conectare!";
                mail.Body = "<p>Bună,</p><br><p>A apărut o nouă conectare de terapeut:</p><br>" +
                                 $"<p>Nume terapeut: {therapist.FirstName} {therapist.LastName}</p>" +
                                 $"<p>Adresa de email a terapeutului: {therapist.User.Email}</p>" +
                                 "<p> Contactează-l pentru a programa interviul</p>";
                mail.IsBodyHtml = true;
                //Setting From , To and CC
                mail.From = new MailAddress("mindexploretest@gmail.com", "Mind Explore");
                mail.To.Add(new MailAddress("mindexploretest@gmail.com"));
                //mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));

                smtpClient.Send(mail);
            }
            else
            {
                throw new Exception(String.Join(",", result.Errors.Select(x => x.Code)));
            }

        }
        public async Task UpdateRole(string userEmail, string newRole)
        {
            var user = await _userManager.FindByNameAsync(userEmail);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            else
            {
                if (newRole == null)
                {
                    throw new Exception("Role doesn't exist");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, newRole);

                }
            }
        }

        public async Task<bool> UpdateById(int id, TherapistModel therapist)
        {
            var therapistDb = await _unitOfWork.Therapists.GetById(id);
            if (therapistDb != null)
            {
                therapistDb.TherapistId = id;
                therapistDb.FirstName = therapist.FirstName;
                therapistDb.LastName = therapist.LastName;
                therapistDb.LicenseNumber = therapist.LicenseNumber;
                therapistDb.LicenseState = therapist.LicenseState;
                therapistDb.Age = therapist.Age;
                therapistDb.Price = therapist.Price;
                therapistDb.NumberOfPatinents = therapist.NumberOfPatinents;
                therapistDb.City = therapist.City;
                therapistDb.IsAvailable = therapist.IsAvailable;
                therapistDb.Quote = therapist.Quote;
                therapistDb.IsMemberOfLGBT = therapist.IsMemberOfLGBT;
                therapistDb.Gender = therapist.Gender;
                therapistDb.IsHungarian = therapist.IsHungarian;
                therapistDb.IsRRoma = therapist.IsRRoma;
                therapistDb.IsMemberOfLGBT = therapist.IsMemberOfLGBT;
                therapistDb.IsNotReligious = therapist.IsNotReligious;
                therapistDb.IsAdded = therapist.IsAdded;
                


                await _unitOfWork.Therapists.UpdateSpecialtiesOfTherapist(id, therapist.Specialties);
                

                await _unitOfWork.Therapists.Update(therapistDb);
                return true;

            }
            else return false;

        }
        public async Task<List<PatientModel>> GetPatients(int therapistId)
        {
            var patients = await _unitOfWork.Therapists.GetPatientsByTherapistId(therapistId);
            var patientList = new List<PatientModel>();
            foreach(var patient in patients)
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
            return patientList;
        }
        public async Task GiveRating(int therapistId, int rating)
        {
            var therapist = await _unitOfWork.Therapists.GetById(therapistId);
            if (therapist.NoOfRatings == null)
                therapist.NoOfRatings = 0;
            if(therapist.Rating == null)
                therapist.Rating = 0;
            therapist.NoOfRatings++;
            therapist.Rating = (therapist.Rating * (therapist.NoOfRatings - 1) + rating) / therapist.NoOfRatings;
            await _unitOfWork.Therapists.Update(therapist);
        }
        public async Task<TherapistModel> GetById(int id)
        {
            var therapist = await _unitOfWork.Therapists.GetById(id);
            if (therapist != null)
            {
                var therapistModel = new TherapistModel
                {
                    Id = therapist.TherapistId,
                    FirstName = therapist.FirstName,
                    LastName = therapist.LastName,
                    Age = therapist.Age,
                    Gender = therapist.Gender,
                    LicenseNumber = therapist.LicenseNumber,
                    LicenseState = therapist.LicenseState,
                    Rating = therapist.Rating,
                    Price = therapist.Price,
                    NumberOfPatinents = therapist.NumberOfPatinents,
                    City = therapist.City,
                    IsAvailable = therapist.IsAvailable,
                    NoOfRatings = therapist.NoOfRatings,
                    Quote = therapist.Quote,
                    IsHungarian = therapist.IsHungarian,
                    IsRRoma = therapist.IsRRoma,
                    IsAdded = therapist.IsAdded,
                    IsMemberOfLGBT = therapist.IsMemberOfLGBT,
                    IsNotReligious = therapist.IsNotReligious,
                    NoHoursPractice = therapist.NoHoursPractice,



                };
                var specialties = await _unitOfWork.Therapists.GetSpecialtiesByTherapistId(id);
                therapistModel.Specialties = specialties;

                return therapistModel;
            }
            else return null;
        }
        public async Task<List<TherapistModel>> GetAll()
        {
            var therapistDb = await _unitOfWork.Therapists.GetAll();
            var list = new List<TherapistModel>();
            foreach (var therapist in therapistDb)
            {
                var therapistModel = new TherapistModel
                {
                    Id = therapist.TherapistId,
                    FirstName = therapist.FirstName,
                    LastName = therapist.LastName,
                    Age = therapist.Age,
                    Gender = therapist.Gender,
                    LicenseNumber = therapist.LicenseNumber,
                    LicenseState = therapist.LicenseState,
                    Rating = therapist.Rating,
                    Price = therapist.Price,
                    NumberOfPatinents = therapist.NumberOfPatinents,
                    City = therapist.City,
                    IsAvailable = therapist.IsAvailable,
                    NoOfRatings = therapist.NoOfRatings,
                    Quote = therapist.Quote,
                    IsHungarian = therapist.IsHungarian,
                    IsRRoma = therapist.IsRRoma,
                    IsAdded = therapist.IsAdded,
                    IsMemberOfLGBT = therapist.IsMemberOfLGBT,
                    IsNotReligious = therapist.IsNotReligious,
                    NoHoursPractice = therapist.NoHoursPractice,
                };
                var specialties = await _unitOfWork.Therapists.GetSpecialtiesByTherapistId(therapist.TherapistId);
                therapistModel.Specialties = specialties;
                list.Add(therapistModel);
            }
            return list;
        }
        public async Task<bool> DeleteById(int id)
        {
            var patientDb = await _unitOfWork.Therapists.GetById(id);
            if (patientDb != null)
            {
                await _unitOfWork.Therapists.DeleteTherapist(patientDb);
                return true;
            }
            else return false;

        }
        public async Task<List<TherapistModel>> GetTherapistsBySpecialties(List<int> specialtiesIds)
        {
            var therapists = await _unitOfWork.Therapists.FilterBySpecialties(specialtiesIds);
            var therapistsModel = new List<TherapistModel>();
            foreach (var therapist in therapists)
            {
                var therapistModel = new TherapistModel
                {
                    Id = therapist.TherapistId,
                    FirstName = therapist.FirstName,
                    LastName = therapist.LastName,
                    Age = therapist.Age,
                    Gender = therapist.Gender,
                    LicenseNumber = therapist.LicenseNumber,
                    LicenseState = therapist.LicenseState,
                    Rating = therapist.Rating,
                    Price = therapist.Price,
                    NumberOfPatinents = therapist.NumberOfPatinents,
                    City = therapist.City,
                    IsAvailable = therapist.IsAvailable,
                    NoOfRatings = therapist.NoOfRatings,
                    Quote = therapist.Quote,
                    IsHungarian = therapist.IsHungarian,
                    IsRRoma = therapist.IsRRoma,
                    IsAdded = therapist.IsAdded,
                    IsMemberOfLGBT = therapist.IsMemberOfLGBT,
                    IsNotReligious = therapist.IsNotReligious,
                    NoHoursPractice = therapist.NoHoursPractice,
                };
                therapistModel.Specialties = await _unitOfWork.Therapists.GetSpecialtiesByTherapistId(therapist.TherapistId);
                therapistsModel.Add(therapistModel);
            }
            return therapistsModel;


        }
        public async Task<List<TherapistModel>> GetTherapistsByAscendingPrice()
        {
            var therapists = await _unitOfWork.Therapists.FilterByAscendingPrice();
            var therapistsModel = new List<TherapistModel>();
            foreach (var therapist in therapists)
            {
                var therapistModel = new TherapistModel
                {
                    Id = therapist.TherapistId,
                    FirstName = therapist.FirstName,
                    LastName = therapist.LastName,
                    Age = therapist.Age,
                    Gender = therapist.Gender,
                    LicenseNumber = therapist.LicenseNumber,
                    LicenseState = therapist.LicenseState,
                    Rating = therapist.Rating,
                    Price = therapist.Price,
                    NumberOfPatinents = therapist.NumberOfPatinents,
                    City = therapist.City,
                    IsAvailable = therapist.IsAvailable,
                    NoOfRatings = therapist.NoOfRatings,
                    Quote = therapist.Quote,
                    IsHungarian = therapist.IsHungarian,
                    IsRRoma = therapist.IsRRoma,
                    IsAdded = therapist.IsAdded,
                    IsMemberOfLGBT = therapist.IsMemberOfLGBT,
                    IsNotReligious = therapist.IsNotReligious,
                    NoHoursPractice = therapist.NoHoursPractice,
                };
                therapistModel.Specialties = await _unitOfWork.Therapists.GetSpecialtiesByTherapistId(therapist.TherapistId);
                therapistsModel.Add(therapistModel);
            }
            return therapistsModel;


        }
        public async Task<List<TherapistModel>> GetTherapistsByDescendingPrice()
        {
            var therapists = await _unitOfWork.Therapists.FilterByDescendingPrice();
            var therapistsModel = new List<TherapistModel>();
            foreach (var therapist in therapists)
            {
                var therapistModel = new TherapistModel
                {
                    Id = therapist.TherapistId,
                    FirstName = therapist.FirstName,
                    LastName = therapist.LastName,
                    Age = therapist.Age,
                    Gender = therapist.Gender,
                    LicenseNumber = therapist.LicenseNumber,
                    LicenseState = therapist.LicenseState,
                    Rating = therapist.Rating,
                    Price = therapist.Price,
                    NumberOfPatinents = therapist.NumberOfPatinents,
                    City = therapist.City,
                    IsAvailable = therapist.IsAvailable,
                    NoOfRatings = therapist.NoOfRatings,
                    Quote = therapist.Quote,
                    IsHungarian = therapist.IsHungarian,
                    IsRRoma = therapist.IsRRoma,
                    IsAdded = therapist.IsAdded,
                    IsMemberOfLGBT = therapist.IsMemberOfLGBT,
                    IsNotReligious = therapist.IsNotReligious,
                    NoHoursPractice = therapist.NoHoursPractice,
                };
                therapistModel.Specialties = await _unitOfWork.Therapists.GetSpecialtiesByTherapistId(therapist.TherapistId);
                therapistsModel.Add(therapistModel);
            }
            return therapistsModel;


        }
        public async Task<List<TherapistModel>> GetTherapistsBySpecialtiesAndAscendingPrice(List<int> specialtiesIds)
        {
            var therapists = await _unitOfWork.Therapists.FilterBySpecialtiesAndAscendingPrice(specialtiesIds);
            var therapistsModel = new List<TherapistModel>();
            foreach (var therapist in therapists)
            {
                var therapistModel = new TherapistModel
                {
                    Id = therapist.TherapistId,
                    FirstName = therapist.FirstName,
                    LastName = therapist.LastName,
                    Age = therapist.Age,
                    Gender = therapist.Gender,
                    LicenseNumber = therapist.LicenseNumber,
                    LicenseState = therapist.LicenseState,
                    Rating = therapist.Rating,
                    Price = therapist.Price,
                    NumberOfPatinents = therapist.NumberOfPatinents,
                    City = therapist.City,
                    IsAvailable = therapist.IsAvailable,
                    NoOfRatings = therapist.NoOfRatings,
                    Quote = therapist.Quote,
                    IsHungarian = therapist.IsHungarian,
                    IsRRoma = therapist.IsRRoma,
                    IsAdded = therapist.IsAdded,
                    IsMemberOfLGBT = therapist.IsMemberOfLGBT,
                    IsNotReligious = therapist.IsNotReligious,
                    NoHoursPractice = therapist.NoHoursPractice,
                };
                therapistModel.Specialties = await _unitOfWork.Therapists.GetSpecialtiesByTherapistId(therapist.TherapistId);
                therapistsModel.Add(therapistModel);
            }
            
            return therapistsModel;
        }
        public async Task<List<TherapistModel>> GetTherapistsBySpecialtiesAndDescendingPrice(List<int>specialtiesIds)
        {
            var therapists = await _unitOfWork.Therapists.FilterBySpecialtiesAndDescendingPrice(specialtiesIds);
            var therapistsModel = new List<TherapistModel>();
            foreach (var therapist in therapists)
            {
                var therapistModel = new TherapistModel
                {
                    Id = therapist.TherapistId,
                    FirstName = therapist.FirstName,
                    LastName = therapist.LastName,
                    Age = therapist.Age,
                    Gender = therapist.Gender,
                    LicenseNumber = therapist.LicenseNumber,
                    LicenseState = therapist.LicenseState,
                    Rating = therapist.Rating,
                    Price = therapist.Price,
                    NumberOfPatinents = therapist.NumberOfPatinents,
                    City = therapist.City,
                    IsAvailable = therapist.IsAvailable,
                    NoOfRatings = therapist.NoOfRatings,
                    Quote = therapist.Quote,
                    IsHungarian = therapist.IsHungarian,
                    IsRRoma = therapist.IsRRoma,
                    IsAdded = therapist.IsAdded,
                    IsMemberOfLGBT = therapist.IsMemberOfLGBT,
                    IsNotReligious = therapist.IsNotReligious,
                    NoHoursPractice = therapist.NoHoursPractice,
                };
                therapistModel.Specialties = await _unitOfWork.Therapists.GetSpecialtiesByTherapistId(therapist.TherapistId);
                therapistsModel.Add(therapistModel);
            }
            return therapistsModel;
        }
        public async Task<List<string>> GetSpecialtiesOfTherapist(int therapistId)
        {
            var specialties = await _unitOfWork.Therapists.GetSpecialties(therapistId);
            var therapistSpecialties = new List<string>();
            foreach(var specialty in specialties)
            {
                therapistSpecialties.Add(specialty.Name);
            }
            return therapistSpecialties;
        }
    }
}
