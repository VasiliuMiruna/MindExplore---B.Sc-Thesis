using therapy_app_backend.BLL.Interfaces;
using therapy_app_backend.BLL.Models;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.DAL.Interfaces;


namespace therapy_app_backend.BLL.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<string> CreateAppointment(int patientId, AppointmentModel appointmentModel)
        {
            var slot = await _unitOfWork.Slots.GetSlotById(appointmentModel.SlotId);
            Console.WriteLine(slot);
            if (slot != null && slot.Available == true)
            {
                var appointment = new Appointment
                {
                    StartTime = TimeZoneInfo.ConvertTime(appointmentModel.StartTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                    EndTime = TimeZoneInfo.ConvertTime(appointmentModel.EndTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                    Status = "Pending",
                    PatientId = patientId,
                    TherapistId = slot.TherapistId,
                    SlotId = appointmentModel.SlotId
                };
                await _unitOfWork.Appointments.CreateAppointment(appointment);
                slot.Available = false;
                await _unitOfWork.Slots.UpdateSlot(slot);
                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    return "Appointment created successfully.";
                }
                else
                {
                    return "Failed to create appointment.";
                }

            }
            else return "Slot null or unavailable";
        }
        public async Task<string> UpdateAppointmentStatus(int appointmentId, string status)
        {
            var appointment = await _unitOfWork.Appointments.GetAppointment(appointmentId);
            if (appointment != null)
            {
                appointment.Status = status;
                try
                {
                    if (appointment.Status == "Accepted")
                    {
                        await _unitOfWork.Appointments.UpdateAppointment(appointment);
                        await _unitOfWork.Therapists.AddPatientToTherapist(appointment.PatientId, appointment.TherapistId);
                        var result = _unitOfWork.Slots.GetSlotById(appointment.SlotId);
                        if (result != null)
                        {
                            result.Result.Available = false;
                            await _unitOfWork.Slots.UpdateSlot(result.Result);

                            return "Appointment updated successfully.";
                        }
                        else return "Error";
                    }
                    else if (appointment.Status == "Rejected")
                    {
                        var slot = new Slot
                        {
                            StartTime = TimeZoneInfo.ConvertTime(appointment.StartTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                            EndTime = TimeZoneInfo.ConvertTime(appointment.EndTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                            Available = true,
                            TherapistId = appointment.TherapistId,
                        };
                        await _unitOfWork.Slots.AddSlot(slot);
                        await _unitOfWork.Appointments.DeleteAppointment(appointment);
                        return "Cleared slot.";
                    }
                    else return "Appointment updated successfully.";

                }
                catch
                {
                    return "Error";
                }
            }
            else
            {
                return "Appointment not found.";
            }
        }

        /*  public async Task<List<AppointmentModel>> GetAllAppointmentsByTherapistId(int therapistId)
          {
              var appointments = await _unitOfWork.Appointments.GetAppointmentsByTherapistId(therapistId);
              var appointmentsModels = new List<AppointmentModel>();
              foreach (var appointment in appointments)
              {
                  var appointmentModel = new AppointmentModel
                  {
                      Id = appointment.Id,
                      *//* StartTime = appointment.StartTime,
                       EndTime = appointment.EndTime,*//*
                      StartTime = TimeZoneInfo.ConvertTime(appointment.StartTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                      EndTime = TimeZoneInfo.ConvertTime(appointment.EndTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                      Location = appointment.Location,
                      Status = appointment.Status,
                      PatientId = appointment.PatientId,
                      TherapistId = appointment.TherapistId,
                      SlotId = appointment.SlotId

                  };
                  appointmentsModels.Add(appointmentModel);
              }
              return appointmentsModels;
          }
          public async Task<List<AppointmentModel>> GetAllAppointmentsByPatientId(int patientId)
          {
              var appointments = await _unitOfWork.Appointments.GetAppointmentsByPatientId(patientId);
              var appointmentsModels = new List<AppointmentModel>();
              foreach (var appointment in appointments)
              {
                  var appointmentModel = new AppointmentModel
                  {
                      Id = appointment.Id,
                      StartTime = TimeZoneInfo.ConvertTime(appointment.StartTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                      EndTime = TimeZoneInfo.ConvertTime(appointment.EndTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                      Location = appointment.Location,
                      Status = appointment.Status,
                      PatientId = appointment.PatientId,
                      TherapistId = appointment.TherapistId,
                      SlotId = appointment.SlotId

                  };
                  appointmentsModels.Add(appointmentModel);
              }
              return appointmentsModels;
          }*/
        public async Task<List<AppointmentModel>> GetAllAppointmentsByTherapistId(int therapistId)
        {
            var appointments = await _unitOfWork.Appointments.GetAppointmentsByTherapistId(therapistId);
            var appointmentsModels = new List<AppointmentModel>();
            foreach (var appointment in appointments)
            {
                var appointmentModel = new AppointmentModel
                {
                    Id = appointment.Id,
                    StartTime = TimeZoneInfo.ConvertTime(appointment.StartTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                    EndTime = TimeZoneInfo.ConvertTime(appointment.EndTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                    Location = appointment.Location,
                    Status = appointment.Status,
                    PatientId = appointment.PatientId,
                    TherapistId = appointment.TherapistId,
                    SlotId = appointment.SlotId

                };
                appointmentsModels.Add(appointmentModel);
            }
            return appointmentsModels;
        }

        public async Task<List<AppointmentModel>> GetAllAppointmentsByPatientId(int patientId)
        {
            var appointments = await _unitOfWork.Appointments.GetAppointmentsByPatientId(patientId);
            var appointmentsModels = new List<AppointmentModel>();
            foreach (var appointment in appointments)
            {
                var appointmentModel = new AppointmentModel
                {
                    Id = appointment.Id,
                    StartTime = TimeZoneInfo.ConvertTime(appointment.StartTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                    EndTime = TimeZoneInfo.ConvertTime(appointment.EndTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                    Location = appointment.Location,
                    Status = appointment.Status,
                    PatientId = appointment.PatientId,
                    TherapistId = appointment.TherapistId,
                    SlotId = appointment.SlotId

                };
                appointmentsModels.Add(appointmentModel);
            }
            return appointmentsModels;
        }
        public async Task DeleteAppointment(int appointmentId)
        {
            var appointment = await _unitOfWork.Appointments.GetAppointment(appointmentId);
            await _unitOfWork.Appointments.DeleteAppointment(appointment);
        }
        public async Task<AppointmentModel> GetAppointment(int appointmentId)
        {
            var appointment = await _unitOfWork.Appointments.GetAppointment(appointmentId);
            var appointmentModel = new AppointmentModel
            {

                Id = appointment.Id,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Location = appointment.Location,
                Status = appointment.Status,
                PatientId = appointment.PatientId,
                TherapistId = appointment.TherapistId,
                SlotId = appointment.SlotId

            };
            return appointmentModel;

        }
    }
}
