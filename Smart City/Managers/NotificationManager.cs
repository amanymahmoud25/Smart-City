using AutoMapper;
using Smart_City.Dtos;
using Smart_City.Models;
using Smart_City.Repositories;

namespace Smart_City.Managers
{
    public class NotificationManager : INotificationManager
    {
        private readonly INotificationsRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public NotificationManager(INotificationsRepository repo, IUserRepository userRepo, IMapper mapper)
        {
            _repo = repo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public bool Create(NotificationCreateDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Message)) return false;
            if (dto.CitizenId == null) return Broadcast(dto.Message);
            return CreateForCitizen(dto.CitizenId.Value, dto.Message);
        }

        public bool CreateForCitizen(int citizenId, string message)
        {
            var citizen = _userRepo.GetCitizenByIdAsync(citizenId).Result; 
            if (citizen == null) return false;

            var notification = _mapper.Map<Notification>(
                new NotificationCreateDto { Message = message, CitizenId = citizenId }
            );
            return _repo.Add(notification);
        }

        public bool Broadcast(string message)
        {
            var citizens = _userRepo.GetCitizensAsync().Result; 
            if (citizens == null || citizens.Count == 0) return false;

            foreach (var citizen in citizens)
            {
                var notification = _mapper.Map<Notification>(
                    new NotificationCreateDto { Message = message, CitizenId = citizen.Id }
                );
                _repo.Add(notification);
            }
            return true;
        }

        public NotificationDto GetById(int id) => _mapper.Map<NotificationDto>(_repo.GetById(id));
        public List<NotificationDto> GetAll() => _mapper.Map<List<NotificationDto>>(_repo.GetAll());
        public List<NotificationDto> GetByCitizenId(int citizenId) => _mapper.Map<List<NotificationDto>>(_repo.GetByCitizenId(citizenId));
        public List<NotificationDto> GetByCitizenName(string citizenName) => _mapper.Map<List<NotificationDto>>(_repo.GetByCitizenName(citizenName));
        public bool Delete(int id) => _repo.Delete(id);
    }
}
