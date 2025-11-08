using Smart_City.Dtos;
using Smart_City.Models;

namespace Smart_City.Managers
{
    public interface IComplaintManager
    {
        Task<ComplaintDto?> CreateAsync(ComplaintCreateDto dto, int citizenId);

        Task<IReadOnlyList<ComplaintDto>> GetByCitizenAsync(
            int citizenId,
            string? status,
            DateTime? from,
            DateTime? to);

        Task<PagedResult<ComplaintDto>> GetAllAsync(
            string? status,
            int? citizenId,
            DateTime? from,
            DateTime? to,
            int page,
            int pageSize);

        Task<ComplaintDto?> GetByIdAsync(int id);

        Task<ComplaintDto?> UpdateStatusAsync(int id, ComplaintStatus? newStatus, int adminId, string? note);

        Task<List<ComplaintDto>> GetAllAsync();
    }

    public record PagedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int PageSize);
}
