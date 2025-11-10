using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Smart_City.Models;
using BCrypt.Net;

namespace Smart_City.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SmartCityContext _context;

        public UserRepository(SmartCityContext context)
        {
            _context = context;
        }

        //  Get All Users
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // Get User By ID
        public  Task<User> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        //  Get User by National ID
        public async Task<User> GetByNationalIdAsync(string nationalId)
        {
            if (string.IsNullOrEmpty(nationalId))
                return null;

            return await _context.Users.FirstOrDefaultAsync(u => u.NationalId == nationalId);
        }

        //  Add new user
        public async Task<bool> AddAsync(User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.NationalId))
                return false;

            bool exists = await _context.Users.AnyAsync(u => u.NationalId == user.NationalId);
            if (exists)
                return false;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        //  Update user
        public async Task<bool> UpdateAsync(User user)
        {
            if (user == null || user.Id <= 0)
                return false;

            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existing == null)
                return false;

            existing.Name = user.Name;
            existing.Email = user.Email;
            existing.Phone = user.Phone;
            existing.PasswordHash = user.PasswordHash;

            await _context.SaveChangesAsync();
            return true;
        }

        //  Delete user
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        //  Get all citizens
        public async Task<List<Citizen>> GetCitizensAsync()
        {
            return await _context.Users.OfType<Citizen>().ToListAsync();
        }

        //  Get all admins
        public async Task<List<Admin>> GetAdminsAsync()
        {
            return await _context.Users.OfType<Admin>().ToListAsync();
        }

        //  Get citizen by ID
        public async Task<Citizen> GetCitizenByIdAsync(int id)
        {
            return await _context.Users.OfType<Citizen>().FirstOrDefaultAsync(c => c.Id == id);
        }

        //  Search users by name
        public async Task<List<User>> SearchByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return new List<User>();

            return await _context.Users
                .Where(u => u.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        //  Login
        public async Task<User> LoginAsync(string nationalId, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.NationalId == nationalId);
            if (user == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return user;
        }

        //  Deactivate user
        public async Task<bool> DeactivateUserAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user == null)
                return false;

            user.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        //  Promote citizen to admin (Async)
        public async Task<bool> PromoteToAdminAsync(int userId)
        {
            var citizen = await _context.Users.OfType<Citizen>().FirstOrDefaultAsync(u => u.Id == userId);
            if (citizen == null)
                return false;

            citizen.Role = "Admin";
            _context.Entry(citizen).Property("Discriminator").CurrentValue = "Admin";

            await _context.SaveChangesAsync();
            return true;
        }

       
        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            if (id <= 0) return null;
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public bool Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return false;
            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

        public bool Update(User user)
        {
            if (user == null || user.Id <= 0) return false;

            var existing = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existing == null) return false;

            existing.Name = user.Name;
            existing.Email = user.Email;
            existing.Phone = user.Phone;
            existing.PasswordHash = user.PasswordHash;

            _context.SaveChanges();
            return true;
        }

        public bool PromoteToAdmin(int userId)
        {
            var citizen = _context.Users.OfType<Citizen>().FirstOrDefault(u => u.Id == userId);
            if (citizen == null) return false;

            citizen.Role = "Admin";
            _context.Entry(citizen).Property("Discriminator").CurrentValue = "Admin";
            _context.SaveChanges();
            return true;
        }

        public List<Citizen> GetCitizens()
        {
            return _context.Users.OfType<Citizen>().ToList();
        }

        public Citizen GetCitizenById(int id)
        {
            if (id <= 0) return null;
            return _context.Users.OfType<Citizen>().FirstOrDefault(c => c.Id == id);
        }
    }
}
