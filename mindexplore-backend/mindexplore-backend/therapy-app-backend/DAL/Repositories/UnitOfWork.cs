using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Patients = new PatientRepository(context);
            Therapists = new TherapistRepository(context);
            Appointments = new AppointmentRepository(context);
            Slots = new SlotRepository(context);
            Specialties = new SpecialtyRepository(context);
            Notes = new NoteRepository(context);
            Tests = new TestRepository(context);
        }

        public IPatientRepository Patients { get; private set; }
        public ITherapistRepository Therapists { get; private set; }
        public IAppointmentRepository Appointments { get; private set; }
        public ISlotRepository Slots { get; private set; }  
        public ISpecialtyRepository Specialties { get; private set; }
        public INoteRepository Notes { get; private set; }
        public ITestRepository Tests { get; private set; }
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
