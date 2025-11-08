using System.Collections.Generic;
using System.Threading.Tasks;
using Smart_City.Dtos;

namespace Smart_City.Managers
{
    public interface IUserManager
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(UserUpdateDto dto);
        Task<bool> PromoteToAdminAsync(int id);
    }
}
