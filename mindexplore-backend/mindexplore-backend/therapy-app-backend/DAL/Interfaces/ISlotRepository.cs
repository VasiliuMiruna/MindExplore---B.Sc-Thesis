using therapy_app_backend.DAL.Entities;

namespace therapy_app_backend.DAL.Interfaces
{
    public interface ISlotRepository
    {
        Task AddSlot(Slot slot);
        Task<List<Slot>> GetSlotsByTherapistId(int therapistId);
        Task<List<Slot>> GetAvailableSlots(int therapistId);
        Task<Slot> GetSlotById(int slotId);
        Task UpdateSlot(Slot slot);
        Task RemoveSlot(Slot slot);
    }
}
