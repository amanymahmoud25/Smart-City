using AutoMapper;
using Smart_City.Dtos;
using Smart_City.Models;
using Smart_City.Repositories;

namespace Smart_City.Managers
{
    public class SuggestionManager : ISuggestionManager
    {
        private readonly ISuggestionsRepositories _repo;
        private readonly IMapper _mapper;
        private readonly INotificationsRepository _notificationRepo;

        public SuggestionManager(
            ISuggestionsRepositories repo,
            IMapper mapper,
            INotificationsRepository notificationRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _notificationRepo = notificationRepo;
        }

        public List<SuggestionDto> GetAll()
        {
            var suggestions = _repo.GetAll();
            return _mapper.Map<List<SuggestionDto>>(suggestions);
        }

        public SuggestionDto GetById(int id)
        {
            var suggestion = _repo.GetById(id);
            return _mapper.Map<SuggestionDto>(suggestion);
        }

        public List<SuggestionDto> GetByCitizenId(int citizenId)
        {
            var list = _repo.GetByCitizenId(citizenId);
            return _mapper.Map<List<SuggestionDto>>(list);
        }

        public SuggestionDto Create(SuggestionCreateDto dto)
        {
            var suggestion = _mapper.Map<Suggestion>(dto);
            suggestion.DateSubmitted = DateTime.Now;
            suggestion.Status = "Pending";

            var saved = _repo.Add(suggestion);

            if (saved)
            {
                _notificationRepo.Add(new Notification
                {
                    CitizenId = suggestion.CitizenId,
                    Message = "Your suggestion has been submitted successfully."
                });

                return _mapper.Map<SuggestionDto>(suggestion);
            }

            return null;
        }

        public SuggestionDto Update(int id, SuggestionUpdateDto dto)
        {
            var suggestion = _repo.GetById(id);
            if (suggestion == null)
                return null;

            suggestion.Title = dto.Title;
            suggestion.Description = dto.Description;
            suggestion.Status = dto.Status;

            var updated = _repo.Update(suggestion);
            if (!updated)
                return null;

            _notificationRepo.Add(new Notification
            {
                CitizenId = suggestion.CitizenId,
                Message = $"Your suggestion #{suggestion.Id} status changed to: {dto.Status}"
            });

            return _mapper.Map<SuggestionDto>(suggestion);
        }

        public bool Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}
