using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using therapy_app_backend.DAL.Entities;


namespace therapy_app_backend
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Therapist> Therapists { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<PatientTherapist> PatientTherapists { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<TherapistSpecialty> TherapistSpecialties { get; set; }
        public DbSet<AppUserRefreshToken> RefreshTokens { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Test> Tests { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var options = new JsonSerializerOptions(JsonSerializerDefaults.General);

           
            modelBuilder.Entity<Match>()
                .ToTable("Matches");

            modelBuilder.Entity<Appointment>()
                .ToTable("Appointments");

            modelBuilder.Entity<Test>()
                .Property(q => q.Questions)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, options),
                    s => JsonSerializer.Deserialize<Dictionary<string,string>>(s, options)!,
                    ValueComparer.CreateDefault(typeof(Dictionary<string, string>), true));

            modelBuilder.Entity<AppUserRefreshToken>()
                                .HasOne(d => d.User)
                                .WithMany(au => au.RefreshTokens)
                                .HasForeignKey(d => d.UserId)
                                .OnDelete(DeleteBehavior.Cascade);


            //one-to-one

            modelBuilder.Entity<Patient>()
                    .HasOne(d => d.User)
                    .WithOne(au => au.Patient)
                    .HasForeignKey<Patient>(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Therapist>()
                    .HasOne(d => d.User)
                    .WithOne(au => au.Therapist)
                    .HasForeignKey<Therapist>(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Patient>()
                 .HasMany(p => p.Appointments)
                 .WithOne(a => a.Patient)
                 .HasForeignKey(a => a.PatientId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Therapist>()
                    .HasMany(t => t.Appointments)
                    .WithOne(a => a.Therapist)
                    .HasForeignKey(a => a.TherapistId)
                    .OnDelete(DeleteBehavior.Cascade);
            //many-to-many
            modelBuilder.Entity<Match>()
                .HasKey(pt => pt.Id); // use Id as primary key



            modelBuilder.Entity<Match>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.Matches)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Therapist)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.TherapistId)
                .OnDelete(DeleteBehavior.NoAction);

            //many-to-many
            modelBuilder.Entity<Appointment>()
                .HasKey(pt => pt.Id); // use Id as primary key



            modelBuilder.Entity<Appointment>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
                .HasOne(m => m.Therapist)
                .WithMany(t => t.Appointments)
                .HasForeignKey(m => m.TherapistId)
                .OnDelete(DeleteBehavior.NoAction);

            //many-to-many
            modelBuilder.Entity<PatientTherapist>()
               .HasKey(pt => pt.Id); // use Id as primary key


            modelBuilder.Entity<PatientTherapist>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.PatientTherapists)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PatientTherapist>()
                .HasOne(m => m.Therapist)
                .WithMany(t => t.PatientTherapists)
                .HasForeignKey(m => m.TherapistId)
                .OnDelete(DeleteBehavior.NoAction);

            //one to many therapist-slots
            modelBuilder.Entity<Therapist>()
                .HasMany(t => t.Slots)
                .WithOne(s => s.Therapist)
                .HasForeignKey(s => s.TherapistId);

            //one on one appointments-slots
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Slot)
                .WithOne(s => s.Appointment)
                .HasForeignKey<Appointment>(a => a.SlotId);

            //many to many specialties-therapists
            modelBuilder.Entity<TherapistSpecialty>()
                .HasKey(ts => ts.Id);

            modelBuilder.Entity<TherapistSpecialty>()
                .HasOne(ts => ts.Therapist)
                .WithMany(t => t.TherapistSpecialties)
                .HasForeignKey(ts => ts.TherapistId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TherapistSpecialty>()
                .HasOne(ts => ts.Specialty)
                .WithMany(s => s.TherapistSpecialties)
                .HasForeignKey(ts => ts.SpecialtyId)
                .OnDelete(DeleteBehavior.NoAction);

            //one to many therapist-slots
            /*modelBuilder.Entity<Therapist>()
                .HasMany(t => t.Slots)
                .WithOne(s => s.Therapist)
                .HasForeignKey(s => s.TherapistId);*/

            //one to many patient-notes

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Notes)
                .WithOne(n => n.Patient)
                .HasForeignKey(n => n.PatientId);


        }
    }
}
