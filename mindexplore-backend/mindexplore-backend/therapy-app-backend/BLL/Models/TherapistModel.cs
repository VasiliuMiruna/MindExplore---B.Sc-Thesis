namespace therapy_app_backend.BLL.Models
{
    public class TherapistModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? LicenseNumber { get; set; }
        public string? LicenseState { get; set; }
        public double? Rating { get; set; }
        public int? NoOfRatings { get; set; }
        public int? NumberOfPatinents { get; set; }
        public string? City { get; set; }
        public int? Price { get; set; }
        public bool? IsAvailable { get; set; }
        public string? Quote { get; set; }
        public int? NoHoursPractice { get; set; }
        public List<int>? Specialties { get; set; }
        public bool? IsRRoma { get; set; }
        public bool? IsHungarian { get; set; }
        public bool? IsMemberOfLGBT { get; set; }
        public bool? IsNotReligious { get; set; }
        public bool? IsAdded { get; set; }
    }
}
