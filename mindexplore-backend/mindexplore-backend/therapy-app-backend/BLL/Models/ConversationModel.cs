using therapy_app_backend.DAL.Entities;

namespace therapy_app_backend.BLL.Models
{
    public class ConversationModel
    {
        public Guid ConversationId { get; set; }
        public string PatientId { get; set; }
        public string TherapistId { get; set; }

        public Therapist Therapist { get; set; }
    }
}
