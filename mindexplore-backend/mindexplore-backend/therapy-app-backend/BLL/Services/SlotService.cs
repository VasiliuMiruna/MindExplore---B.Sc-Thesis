using therapy_app_backend.BLL.Interfaces;
using therapy_app_backend.BLL.Models;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.BLL.Services
{
    public class SlotService : ISlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SlotService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreateSlotForTherapist(int id, SlotModel slot)
        {
            var therapist = await _unitOfWork.Therapists.GetById(id);
            if (therapist == null)
            {
                throw new Exception("Therapist not found.");
            }
            else
            {
                Slot slotDb = new Slot
                {
                    /*StartTime = slot.StartTime,*/
                    StartTime = TimeZoneInfo.ConvertTime(slot.StartTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                    EndTime = TimeZoneInfo.ConvertTime(slot.EndTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                    Available = slot.Available,
                };
                slotDb.TherapistId = id;
                await _unitOfWork.Slots.AddSlot(slotDb);
            }
        }
        public async Task DeleteSlot(int slotId)
        {
            var slot = await _unitOfWork.Slots.GetSlotById(slotId);
            if (slot == null)
            {
                throw new Exception("Slot not found.");
            }
            else
            {
                await _unitOfWork.Slots.RemoveSlot(slot);
            }
        }
        /* public async Task<List<SlotModel>> GetAllSlots(int id)
         {
             var slots = await _unitOfWork.Slots.GetSlotsByTherapistId(id);
             var slotsModel = new List<SlotModel>();
             foreach (var slot in slots)
             {
                 var slotM = new SlotModel
                 {
                     Id = slot.Id,
                     *//* StartTime = slot.StartTime,
                      EndTime = slot.EndTime,*//*
                     StartTime = TimeZoneInfo.ConvertTime(slot.StartTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                     EndTime = TimeZoneInfo.ConvertTime(slot.EndTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),

                     Available = slot.Available
                 };
                 slotsModel.Add(slotM);

             }
             return slotsModel;
         }*/
        public async Task<List<SlotModel>> GetAllSlots(int id)
        {
            var slots = await _unitOfWork.Slots.GetSlotsByTherapistId(id);
            var slotsModel = new List<SlotModel>();
            foreach (var slot in slots)
            {
                var slotM = new SlotModel
                {
                    Id = slot.Id,
                    StartTime = TimeZoneInfo.ConvertTime(slot.StartTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                    EndTime = TimeZoneInfo.ConvertTime(slot.EndTime, TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest")),
                  
                    Available = slot.Available
                };
                slotsModel.Add(slotM);
            }
            return slotsModel;
        }

        public async Task<List<SlotModel>> GetAllAvailableSlots(int id)
        {
            var slots = await _unitOfWork.Slots.GetAvailableSlots(id);
            var slotsModel = new List<SlotModel>();
            foreach (var slot in slots)
            {
                var slotM = new SlotModel
                {
                    Id= slot.Id,
                    StartTime = slot.StartTime,
                    EndTime = slot.EndTime,
                    Available = slot.Available
                };
                slotsModel.Add(slotM);

            }
            return slotsModel;
        }

    }
}
