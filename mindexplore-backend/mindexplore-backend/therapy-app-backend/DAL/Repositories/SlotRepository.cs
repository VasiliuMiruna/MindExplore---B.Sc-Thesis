using Microsoft.EntityFrameworkCore;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.DAL.Repositories
{
    public class SlotRepository : ISlotRepository
    {
        private readonly AppDbContext _db;

        public SlotRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task AddSlot(Slot slot)
        {
            _db.Slots.Add(slot);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Slot>> GetSlotsByTherapistId(int therapistId)
        {
            var slots = await _db.Slots
                .Where(s => s.TherapistId == therapistId)
                .ToListAsync();
            return slots;
        }
        public async Task<List<Slot>> GetAvailableSlots(int therapistId)
        {
            var slots = await _db.Slots
                .Where(s => s.Available == true && s.TherapistId == therapistId)
                .ToListAsync();
            return slots;
        }

        public async Task<Slot> GetSlotById(int slotId)
        {
            var slot = await _db.Slots.FirstOrDefaultAsync(s => s.Id == slotId);
            return slot;
        }

        public async Task UpdateSlot(Slot slot)
        {
            _db.Slots.Update(slot);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveSlot(Slot slot)
        {
            _db.Slots.Remove(slot);
            await _db.SaveChangesAsync();
        }
    }
}
