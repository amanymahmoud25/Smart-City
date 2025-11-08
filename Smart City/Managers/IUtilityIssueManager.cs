using Smart_City.Dtos;
using Smart_City.Models;
using System.Collections.Generic;

namespace Smart_City.Managers
{
    public interface IUtilityIssueManager
    {
        List<UtilityIssueDto> GetAll();
        UtilityIssueDto GetById(int id);
        List<UtilityIssueDto> GetByCitizenId(int citizenId);

        UtilityIssueDto Create(UtilityIssueCreateDto dto, int citizenId);

        UtilityIssueDto Update(UtilityIssueUpdateDto dto);

        bool Delete(int id);
        bool MarkAsResolved(int id);
        List<UtilityIssueDto> GetByType(UtilityIssueType type);
        List<UtilityIssueDto> GetByStatus(string status);
    }
}
