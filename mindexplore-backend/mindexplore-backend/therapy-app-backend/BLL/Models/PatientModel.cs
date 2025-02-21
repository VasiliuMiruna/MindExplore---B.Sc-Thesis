namespace therapy_app_backend.BLL.Models
{
    public class PatientModel
    {
        public int Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public int? Age { get; set; }
        public string? City { get; set; }
    }
}
