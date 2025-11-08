using Smart_City.Dtos;

namespace Smart_City.Managers
{
    public interface ISuggestionManager
    {
        List<SuggestionDto> GetAll();
        SuggestionDto GetById(int id);
        List<SuggestionDto> GetByCitizenId(int citizenId);
        SuggestionDto Create(SuggestionCreateDto dto);
        SuggestionDto Update(int id, SuggestionUpdateDto dto);
        bool Delete(int id);
    }
}
