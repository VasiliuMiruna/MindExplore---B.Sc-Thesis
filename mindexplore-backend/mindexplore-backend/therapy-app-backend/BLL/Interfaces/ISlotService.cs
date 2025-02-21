using therapy_app_backend.BLL.Models;

namespace therapy_app_backend.BLL.Interfaces
{
    public interface ISlotService
    {
        Task CreateSlotForTherapist(int id, SlotModel slot);
        Task<List<SlotModel>> GetAllSlots(int id);
        Task<List<SlotModel>> GetAllAvailableSlots(int id);
        Task DeleteSlot(int slotId);
    }
}
