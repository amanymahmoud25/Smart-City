using Smart_City.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Smart_City.Repositories
{
    public interface IUserRepository
    {
    
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> GetByNationalIdAsync(string nationalId);
        Task<bool> AddAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<List<Citizen>> GetCitizensAsync();
        Task<List<Admin>> GetAdminsAsync();
        Task<Citizen> GetCitizenByIdAsync(int id);
        Task<List<User>> SearchByNameAsync(string name);
        Task<User> LoginAsync(string nationalId, string password);
        Task<bool> DeactivateUserAsync(int id);
        Task<bool> PromoteToAdminAsync(int userId);

        List<User> GetAll();
        User GetById(int id);
        bool Delete(int id);
        bool Update(User user);
        bool PromoteToAdmin(int userId);
        List<Citizen> GetCitizens();
        Citizen GetCitizenById(int id);
    }
}
