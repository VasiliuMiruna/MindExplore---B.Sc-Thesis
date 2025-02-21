using therapy_app_backend.BLL.Models;

namespace therapy_app_backend.BLL.Interfaces
{
    public interface IAppointmentService
    {
        Task<string> CreateAppointment(int patientId, AppointmentModel appointmentModel);
        Task<string> UpdateAppointmentStatus(int appointmentId, string status);
        Task<List<AppointmentModel>> GetAllAppointmentsByTherapistId(int therapistId);
        Task<List<AppointmentModel>> GetAllAppointmentsByPatientId(int patientId);
        Task DeleteAppointment(int appointmentId);
        Task<AppointmentModel> GetAppointment(int appointmentId);
    }
}
