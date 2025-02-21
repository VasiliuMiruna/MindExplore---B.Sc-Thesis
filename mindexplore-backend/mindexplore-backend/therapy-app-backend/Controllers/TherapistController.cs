using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using therapy_app_backend.BLL.Interfaces;
using therapy_app_backend.BLL.Models;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapistController : ControllerBase
    {

        private readonly ITherapistService _therapistService;
        private readonly IAppointmentService _appointmentService;
        private readonly ISlotService _slotService;
        private readonly IUserRepository _userRepository;

        public TherapistController(ITherapistService therapistService, IAppointmentService appointmentService, ISlotService slotService, IUserRepository userRepository)
        {
            _therapistService = therapistService;
            _appointmentService = appointmentService;
            _slotService = slotService;
            _userRepository = userRepository;
        }
        // [Authorize(Roles = "Therapist, Admin")]
        [HttpPost("RegisterTherapist")] 
        public async Task<IActionResult> RegisterTherapist([FromBody] TherapistRegisterModel registerModel)
        {
            try
            {
                Console.WriteLine(registerModel);
                await _therapistService.CreateTherapist(registerModel);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpPut("{therapistId}/add")]
        public async Task AddTherapist([FromRoute] int therapistId)
        {
            var therapist = await _therapistService.GetById(therapistId);
            therapist.IsAdded = true;
            await _therapistService.UpdateById(therapistId, therapist);
        }

        /* [HttpPut("changeRole")]
         public async Task UpdateRole([FromBody] string userEmail, [FromBody]  string newRole)
         {
             await _therapistService.UpdateRole(userEmail, newRole);
         }*/
        [Authorize(Roles = "Therapist, Admin")]
        [HttpPut("{userId}")]
        public async Task UpdateTherapist([FromRoute] string userId, [FromBody] TherapistModel therapist)
        {
            var id = _userRepository.GetShortIdByUserId(userId, "Therapist");
            await _therapistService.UpdateById(id, therapist);
        }
        //[Authorize(Roles = "Patient, Admin")]
        [HttpPut("{id}/{rating}/rating")]
        public async Task UpdateRating([FromRoute] int id,[FromRoute] int rating)
        {
            await _therapistService.GiveRating(id, rating);
        }
       
        [HttpGet("{id}")]
        public async Task<TherapistModel> GetTherapist([FromRoute] int id)
        {
            var therapist = await _therapistService.GetById(id);
            return therapist;


        }
        [HttpGet]
        public async Task<List<TherapistModel>> GetTherapists()
        {
            var therapists = await _therapistService.GetAll();
            List<TherapistModel> list = new List<TherapistModel>();
            foreach (var therapist in therapists)
            {
                if (therapist.IsAdded == true || therapist.IsAdded == null)
                    list.Add(therapist);
            }
            return list;

        }
        [HttpGet("notAdded")]
        public async Task<List<TherapistModel>> GetTherapistsNotAdded()
        {
            var therapists = await _therapistService.GetAll();
            List<TherapistModel> list = new List<TherapistModel>();
            foreach (var therapist in therapists)
            {
                if (therapist.IsAdded == false)
                    list.Add(therapist);
            }
            return list;

        }
        [Authorize(Roles = "Therapist, Admin")]
        [HttpDelete("{id}")]
        public async Task DeleteTherapist([FromRoute] int id)
        {
            await _therapistService.DeleteById(id);
        }
        //[Authorize(Roles = "Therapist, Admin")]
        [HttpPost("{therapistId}/slots")]
        public async Task<IActionResult> CreateSlotForTherapist([FromRoute]int therapistId, [FromBody] SlotModel slot)
        {
            try
            {
                await _slotService.CreateSlotForTherapist(therapistId, slot);
                return Ok(slot);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[Authorize(Roles ="Therapist")]
        [HttpDelete("{therapistId}/{slotId}/slots")]
        public async Task<IActionResult> DeleteSlot([FromRoute] int therapistId, [FromRoute] int slotId)
        {
            try
            {
                await _slotService.DeleteSlot(slotId);
                return Ok("Slot deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       //[Authorize(Roles = "Therapist, Admin")]
        [HttpGet("{therapistId}/slots")]
        public async Task<IActionResult> GetAllSlots([FromRoute] int therapistId)
        {
            var slots = await _slotService.GetAllAvailableSlots(therapistId);
            return Ok(slots);
        }
        //[Authorize(Roles = "Therapist, Patient, Admin")]
        [HttpGet("{therapistId}/slots/available")]
        public async Task<IActionResult> GetAllAvailableSlotsForTherapist([FromRoute] int therapistId)
        {
            var slots = await _slotService.GetAllAvailableSlots(therapistId);
            return Ok(slots);

        }
        //[Authorize(Roles = "Therapist, Admin")]
        [HttpGet("{therapistId}/appointments")]
        public async Task<IActionResult> GetTherapistAppointments([FromRoute] int therapistId)
        {
            var appointments = await _appointmentService.GetAllAppointmentsByTherapistId(therapistId);
            return Ok(appointments);
        }
       // [Authorize(Roles = "Therapist, Admin")]
        [HttpPut("{therapistId}/appointments/{appointmentId}")]
        public async Task<IActionResult> UpdateAppointment([FromRoute] int therapistId, [FromRoute] int appointmentId, [FromBody] string status)
        {
            var result = await _appointmentService.UpdateAppointmentStatus(appointmentId, status);

            if (result == "Appointment updated successfully.")
            {
                return Ok(result);
            }
            else if(result == "Cleared slot.")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("{therapistId}/patients")]
        public async Task<List<PatientModel>> GetPatientsByTherapistId([FromRoute] int therapistId)
        {
            var patients = await _therapistService.GetPatients(therapistId);
            return patients;
        }
        
        [HttpGet("byAscendingPrice")]
        public async Task<List<TherapistModel>> GetTherapistsByAscendingPrice()
        {
            var therapists = await _therapistService.GetTherapistsByAscendingPrice();
            return therapists;
        }

        [HttpGet("byDescendingPrice")]
        public async Task<List<TherapistModel>> GetTherapistsByDescendingPrice()
        {
            var therapists = await _therapistService.GetTherapistsByDescendingPrice();
            return therapists;
        }

        [HttpGet("bySpecialty")]
        public async Task<List<TherapistModel>> GetTherapistsBySpecialty([FromQuery] List<int> specialtyIds)
        {
            var therapists = await _therapistService.GetTherapistsBySpecialties(specialtyIds);
            return therapists;
        }

        [HttpGet("bySpecialtyAndAscendingPrice")]
        public async Task<List<TherapistModel>> GetTherapistsBySpecialtyAndAscendingPrice([FromQuery] List<int> specialtyIds)
        {
            var therapists = await _therapistService.GetTherapistsBySpecialtiesAndAscendingPrice(specialtyIds);
            return therapists;
        }
        [HttpGet("bySpecialtyAndDescendingPrice")]
        public async Task<List<TherapistModel>> GetTherapistsBySpecialtyAndDescendingPrice([FromQuery] List<int> specialtyIds)
        {
            var therapists = await _therapistService.GetTherapistsBySpecialtiesAndDescendingPrice(specialtyIds);
            return therapists;
        }

        [HttpGet("therapistSpecialties/{therapistId}")]
        public async Task<List<string>> GetSpecialtiesOfTherapist(int therapistId)
        {
            var specialties = await _therapistService.GetSpecialtiesOfTherapist(therapistId);
            return specialties;
        }

    }
}
