using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Smart_City.Dtos;
using Smart_City.Models;
using Smart_City.Repositories;

namespace Smart_City.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UserManager(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<bool> UpdateAsync(UserUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(dto.Id);
            if (existing == null) return false;

            existing.Name = dto.Name ?? existing.Name;
            existing.Email = dto.Email ?? existing.Email;
            existing.Phone = dto.Phone ?? existing.Phone;

            return await _repo.UpdateAsync(existing);
        }

        public async Task<bool> PromoteToAdminAsync(int id)
        {
            return await _repo.PromoteToAdminAsync(id);
        }
    }
}
