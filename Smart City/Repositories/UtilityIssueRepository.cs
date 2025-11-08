using Smart_City.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Smart_City.Repositories
{
    public class UtilityIssueRepository :IUtilityIssueRepository
    {
        private readonly SmartCityContext _context;

        public UtilityIssueRepository(SmartCityContext context)
        {
            _context = context;
        }

        public List<UtilityIssue> GetAll()
        {
            return _context.UtilityIssues.Include(i => i.Citizen).ToList();
        }

        public UtilityIssue GetById(int id)
        {
            if (id <= 0)
                return null;

            return _context.UtilityIssues.Include(i => i.Citizen).FirstOrDefault(i => i.Id == id);
        }

        public List<UtilityIssue> GetByCitizenId(int citizenId)
        {
            if (citizenId <= 0)
                return new List<UtilityIssue>();

            return _context.UtilityIssues
                .Include(i => i.Citizen)
                .Where(i => i.CitizenId == citizenId)
                .ToList();
        }

        public bool Add(UtilityIssue issue)
        {
            if (issue == null)
                return false;

            if (string.IsNullOrEmpty(issue.Description))
                return false;

            _context.UtilityIssues.Add(issue);
            _context.SaveChanges();
            return true;
        }

        public bool Update(UtilityIssue issue)
        {
            if (issue == null || issue.Id <= 0)
                return false;

            var existing = _context.UtilityIssues.FirstOrDefault(i => i.Id == issue.Id);
            if (existing == null)
                return false;

            existing.Type = issue.Type;
            existing.Description = issue.Description;
            existing.Status = issue.Status;
            existing.ReportDate = issue.ReportDate;
            existing.CitizenId = issue.CitizenId;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                return false;

            var issue = _context.UtilityIssues.FirstOrDefault(i => i.Id == id);
            if (issue == null)
                return false;

            _context.UtilityIssues.Remove(issue);
            _context.SaveChanges();
            return true;
        }

        public List<UtilityIssue> GetByType(UtilityIssueType type)
        {
            return _context.UtilityIssues
                .Include(i => i.Citizen)
                .Where(i => i.Type == type)
                .ToList();
        }

        public List<UtilityIssue> GetByStatus(string status)
        {
            if (string.IsNullOrEmpty(status))
                return new List<UtilityIssue>();

            return _context.UtilityIssues
                .Include(i => i.Citizen)
                .Where(i => i.Status.ToLower() == status.ToLower())
                .ToList();
        }

        public bool MarkAsResolved(int id)
        {
            var issue = _context.UtilityIssues.FirstOrDefault(i => i.Id == id);
            if (issue == null)
                return false;

            issue.Status = "Resolved";
            _context.SaveChanges();
            return true;
        }


    }
}
