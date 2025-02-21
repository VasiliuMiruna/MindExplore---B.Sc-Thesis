using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace therapy_app_backend.DAL.Entities
{
    public class Slot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Available { get; set; }
        public int TherapistId { get; set; }
        public Therapist Therapist { get; set; }

       
        public int? AppointmentId { get; set; } 
        public Appointment? Appointment { get; set; } 

    }
}
