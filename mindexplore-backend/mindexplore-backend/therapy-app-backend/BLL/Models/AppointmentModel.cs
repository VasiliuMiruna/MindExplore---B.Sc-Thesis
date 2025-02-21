namespace therapy_app_backend.BLL.Models
{
    public class AppointmentModel
    {

        public int? Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Location { get; set; }
        public string? Status { get; set; }
        public int? TherapistId { get; set; }
        public int? PatientId { get; set; }
        public int SlotId { get; set; }
    }

   
}
