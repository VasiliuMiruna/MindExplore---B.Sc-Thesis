namespace therapy_app_backend.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IPatientRepository Patients { get; }
        ITherapistRepository Therapists { get; }
        IAppointmentRepository Appointments { get; }
        ISlotRepository Slots { get; }
        ISpecialtyRepository Specialties { get; }
        INoteRepository Notes { get; }
        ITestRepository Tests { get; }
        Task<int> SaveChangesAsync();
    }
}
