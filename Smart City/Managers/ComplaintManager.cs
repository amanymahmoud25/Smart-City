using AutoMapper;
using Smart_City.Dtos;
using Smart_City.Models;
using Smart_City.Repositories;

namespace Smart_City.Managers
{
    public class ComplaintManager : IComplaintManager
    {
        private readonly IComplaintRepositry _repo;
        private readonly IMapper _map;

        public ComplaintManager(IComplaintRepositry repo, IMapper map)
        {
            _repo = repo;
            _map = map;
        }

        public async Task<ComplaintDto?> CreateAsync(ComplaintCreateDto dto, int citizenId)
        {
            var entity = _map.Map<Complaint>(dto);
            entity.CitizenId = citizenId;
            entity.Status = ComplaintStatus.Pending;
            entity.DateSubmitted = DateTime.UtcNow;

            await _repo.AddAsync(entity);
            return _map.Map<ComplaintDto>(entity);
        }

        public async Task<IReadOnlyList<ComplaintDto>> GetByCitizenAsync(
            int citizenId,
            string? status,
            DateTime? from,
            DateTime? to)
        {
            var list = await _repo.GetByCitizenIdAsync(citizenId);

         
            if (!string.IsNullOrWhiteSpace(status)
                && Enum.TryParse<ComplaintStatus>(status, true, out var st))
            {
                list = list.Where(c => c.Status == st).ToList();
            }

            if (from.HasValue) list = list.Where(c => c.DateSubmitted >= from.Value).ToList();
            if (to.HasValue) list = list.Where(c => c.DateSubmitted <= to.Value).ToList();

            return _map.Map<IReadOnlyList<ComplaintDto>>(list);
        }

        public async Task<PagedResult<ComplaintDto>> GetAllAsync(
            string? status,
            int? citizenId,
            DateTime? from,
            DateTime? to,
            int page,
            int pageSize)
        {
            var all = await _repo.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(status)
                && Enum.TryParse<ComplaintStatus>(status, true, out var st))
            {
                all = all.Where(c => c.Status == st).ToList();
            }

            if (citizenId.HasValue) all = all.Where(c => c.CitizenId == citizenId.Value).ToList();
            if (from.HasValue) all = all.Where(c => c.DateSubmitted >= from.Value).ToList();
            if (to.HasValue) all = all.Where(c => c.DateSubmitted <= to.Value).ToList();

            var total = all.Count;

            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var items = all
                .OrderByDescending(c => c.DateSubmitted)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<ComplaintDto>(
                _map.Map<List<ComplaintDto>>(items),
                total,
                page,
                pageSize
            );
        }

        public async Task<List<ComplaintDto>> GetAllAsync()
        {
            var all = await _repo.GetAllAsync();
            var ordered = all.OrderByDescending(c => c.DateSubmitted).ToList();
            return _map.Map<List<ComplaintDto>>(ordered);
        }

        public async Task<ComplaintDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return _map.Map<ComplaintDto?>(entity);
        }

        public async Task<ComplaintDto?> UpdateStatusAsync(int id, ComplaintStatus? newStatus, int adminId, string? note)
        {
            if (!newStatus.HasValue) return null;

            var entity = await _repo.UpdateStatusAsync(id, newStatus.Value, adminId, note);
            return _map.Map<ComplaintDto?>(entity);
        }
    }
}
