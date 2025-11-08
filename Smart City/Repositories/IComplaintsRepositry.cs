using Smart_City.Models;

namespace Smart_City.Repositories
{
    public interface IComplaintRepositry
    {
        Task<List<Complaint>> GetAllAsync();
        Task<Complaint?> GetByIdAsync(int id);
        Task<List<Complaint>> GetByCitizenIdAsync(int citizenId);

        Task<bool> AddAsync(Complaint complaint);
        Task<bool> UpdateAsync(Complaint complaint);
        Task<bool> UpdateStatusAsync(int id, ComplaintStatus status, int adminId, string? note = null);
        Task<bool> DeleteAsync(int id);

        Task<List<Complaint>> GetUnresolvedAsync(); 
    }
}
