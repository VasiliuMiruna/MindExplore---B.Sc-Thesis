using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace therapy_app_backend.DAL.Entities
{
    public class Therapist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TherapistId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public string? LicenseNumber { get; set; }
        public string? LicenseState { get; set; }
        public double? Rating { get; set; }
        public int? NoOfRatings { get; set; }
        public int? NumberOfPatinents { get; set; }
        public string? City { get; set; }
        public bool? OffersOnlineSessions { get; set; }
        public int? Price { get; set; }
        public bool? IsAvailable { get; set; }    
        public string? Quote { get; set; }
        public int? NoHoursPractice { get; set; }
        public bool? IsRRoma { get; set; }
        public bool? IsHungarian { get; set; }
        public bool? IsMemberOfLGBT { get; set; }
        public bool? IsNotReligious { get; set; }
        public bool? IsAdded { get; set; }
        public virtual ICollection<Match>? Matches { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
        public virtual ICollection<PatientTherapist>? PatientTherapists { get; set; }
        public virtual ICollection<Slot>? Slots { get; set; }
        public virtual ICollection<TherapistSpecialty>? TherapistSpecialties { get; set; }
        public virtual string UserId { get; set; }
        public virtual AppUser User { get; set; }

    }
}
