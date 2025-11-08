using Microsoft.EntityFrameworkCore;
using Smart_City.Models;

namespace Smart_City.Repositories
{
    public interface ISuggestionsRepositories
    {
         List<Suggestion> GetAll();
         Suggestion GetById(int id);
         List<Suggestion> GetByCitizenId(int citizenId);
         bool Add(Suggestion suggestion);
         bool Update(Suggestion suggestion);
         bool Delete(int id);
         List<Suggestion> GetByStatus(string status);
       

    }
}


