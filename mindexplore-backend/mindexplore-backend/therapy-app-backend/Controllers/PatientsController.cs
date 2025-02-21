using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using therapy_app_backend.BLL.Interfaces;
using therapy_app_backend.BLL.Models;
using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly INoteService _noteService;
        private readonly IUserRepository _userRepository;


        public PatientsController(IPatientService patientService, IAppointmentService appointmentService, INoteService noteService, IUserRepository userRepository)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _noteService = noteService;
            _userRepository = userRepository;
        }
        [Authorize(Roles = "Patient, Admin")]
        [HttpPut("{userId}")]
        public async Task UpdatePatient([FromRoute] string userId, [FromBody] PatientModel patient)
        {
            var id = _userRepository.GetShortIdByUserId(userId, "Patient");
            await _patientService.UpdateById(id, patient);
        }
       
        [HttpGet("{id}")]
        public async Task<PatientModel> GetPatient([FromRoute] int id)
        {
            var patient = await _patientService.GetById(id);
            return patient;
        }
        [Authorize(Roles = "Therapist, Admin")]
        [HttpGet]
        public async Task<List<PatientModel>> GetPatients()
        {
            var patients = await _patientService.GetAll();

            return patients;

        }

        [Authorize(Roles = "Patient, Admin")]
        [HttpDelete("{id}")]
        public async Task DeletePatient([FromRoute] int id)
        {
            await _patientService.DeleteById(id);
        }

        // [Authorize(Roles = "Patient, Therapist, Admin")]
        [HttpPost("{patientId}/appointments")]
        public async Task<IActionResult> CreateAppointment([FromRoute] int patientId, [FromBody] AppointmentModel appointmentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var result = await _appointmentService.CreateAppointment(patientId, appointmentModel);

            if (result == "Appointment created successfully.")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        /*
                [HttpDelete("/appointments/{therapistId}/{appointmentId}")]
                public async Task CancelAppointment([FromRoute] int therapistId, [FromRoute]int appointmentId)
                {

                }*/
        //[Authorize(Roles = "Patient, Admin")]
        [HttpGet("{patientId}/appointments")]
        public async Task<IActionResult> GetPatientAppointments(int patientId)
        {
            var appointments = await _appointmentService.GetAllAppointmentsByPatientId(patientId);
            return Ok(appointments);
        }
        //[Authorize(Roles = "Patient, Therapist, Admin")]
        [HttpDelete("/appointments/{appointmentId}")]
        public async Task<IActionResult> RemoveAppointment([FromRoute] int appointmentId)
        {
            await _appointmentService.DeleteAppointment(appointmentId);
            return Ok();
        }


        [HttpPost("{patientId}/notes")]
        public async Task<IActionResult> CreateNote(int patientId, [FromBody] NoteModel noteModel)
        {
            try
            {
                //var id = _userRepository.GetShortIdByUserId(patientId, "Patient");
                await _noteService.CreateNoteForPatient(patientId, noteModel);
                return Ok("Note created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{patientId}/notes")]
        public async Task<List<NoteModel>> GetAllNotes(string patientId)
        {
            var id = _userRepository.GetShortIdByUserId(patientId, "Patient");
            var notes = await _noteService.GetAllNotes(id);
            return notes;
        }
        [Authorize(Roles = "Patient, Admin")]
        [HttpPut("{patientId}/notes/{noteId}")]
        public async Task<IActionResult> UpdateNote([FromRoute] int patientId, NoteModel noteModel, [FromRoute] int noteId)
        {
            try
            {
                await _noteService.UpdateNote(noteId, noteModel);
                return Ok("Note updated succesfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Patient, Admin")]
        [HttpDelete("{patientId}/notes/{noteId}")]
        public async Task<IActionResult> DeleteNote([FromRoute] int patientId, [FromRoute] int noteId)
        {
            try
            {
                await _noteService.DeleteNote(noteId);
                return Ok("Note deleted succesfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Patient")]
        [HttpGet("{patientId}/getFeelingStatistic")]
        public async Task<FeelingModel> GetStatistic(int patientId)
        { 
            var feeling = await _noteService.GetFeelingStatistic(patientId);
            return feeling;

        }
        [HttpGet("{patientId}/therapists")]
        public async Task<List<TherapistModel>> GetPatientsTherapists(int patientId)
        {
            var therapists = await _patientService.GetPatientsTherapists(patientId);
            return therapists;
        }


    }
}
