using therapy_app_backend.DAL.Entities;

namespace therapy_app_backend.DAL.Interfaces
{
    public interface IAppointmentRepository
    {
        Task CreateAppointment(Appointment appointment);
        Task UpdateAppointment(Appointment appointment);
        Task<Appointment> GetAppointment(int appointmentId);
        Task<List<Appointment>> GetAppointmentsByTherapistId(int therapistId);
        Task<List<Appointment>> GetAppointmentsByPatientId(int patientId);
        Task DeleteAppointment(Appointment appointment);
    }
}
