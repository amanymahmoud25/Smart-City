using Smart_City.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Smart_City.Repositories
{
    public class SuggestionsRepositories :ISuggestionsRepositories
    {
        private readonly SmartCityContext _context;
        public SuggestionsRepositories(SmartCityContext context)
        {
            _context = context;
        }
        //  Get all suggestions
        public List<Suggestion> GetAll()
        {
            return _context.Suggestions.Include(s => s.Citizen).ToList();
        }
        //  Get suggestion by ID
        public Suggestion GetById(int id)
        {
            if (id <= 0)
                return null;

            return _context.Suggestions.Include(s => s.Citizen).FirstOrDefault(s => s.Id == id);
        }

        //  Get suggestions by citizen ID
        public List<Suggestion> GetByCitizenId(int citizenId)
        {
            if (citizenId <= 0)
                return new List<Suggestion>();

            return _context.Suggestions
                .Include(s => s.Citizen)
                .Where(s => s.CitizenId == citizenId)
                .ToList();
        }
        //  Add new suggestion
        public bool Add(Suggestion suggestion)
        {
            if (suggestion == null)
                return false;

            if (string.IsNullOrEmpty(suggestion.Description) || suggestion.CitizenId <= 0)
                return false;

            _context.Suggestions.Add(suggestion);
            _context.SaveChanges();
            return true;
        }

        //  Update suggestion 
        public bool Update(Suggestion suggestion)
        {
            if (suggestion == null || suggestion.Id <= 0)
                return false;

            var existing = _context.Suggestions.FirstOrDefault(s => s.Id == suggestion.Id);
            if (existing == null)
                return false;

            existing.Title = suggestion.Title;
            existing.Description = suggestion.Description;
            existing.Status = suggestion.Status;
           
            _context.SaveChanges();
            return true;
        }
        //  Delete suggestion
        public bool Delete(int id)
        {
            if (id <= 0)
                return false;

            var suggestion = _context.Suggestions.FirstOrDefault(s => s.Id == id);
            if (suggestion == null)
                return false;

            _context.Suggestions.Remove(suggestion);
            _context.SaveChanges();
            return true;
        }
        //  Get suggestions by status 
        public List<Suggestion> GetByStatus(string status)
        {
            if (string.IsNullOrEmpty(status))
                return new List<Suggestion>();

            return _context.Suggestions
                .Include(s => s.Citizen)
                .Where(s => s.Status.ToLower() == status.ToLower())
                .ToList();
        }

    }
}
