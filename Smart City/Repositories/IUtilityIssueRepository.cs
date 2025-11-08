using Microsoft.EntityFrameworkCore;
using Smart_City.Models;

namespace Smart_City.Repositories
{
    public interface IUtilityIssueRepository
    {
        
         List<UtilityIssue> GetAll();
         UtilityIssue GetById(int id);
         List<UtilityIssue> GetByCitizenId(int citizenId);
         bool Add(UtilityIssue issue);
         bool Update(UtilityIssue issue);
         bool Delete(int id);
         List<UtilityIssue> GetByType(UtilityIssueType type);
         List<UtilityIssue> GetByStatus(string status);
         bool MarkAsResolved(int id);

    }
}

