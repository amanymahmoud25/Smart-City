using Microsoft.EntityFrameworkCore;
using Smart_City.Managers;
using Smart_City.Models;

namespace Smart_City.Repositories
{
	public class ComplaintsRepositry : IComplaintRepositry
	{
		private readonly SmartCityContext _context;
		private readonly INotificationManager _notificationManager;
		public ComplaintsRepositry(SmartCityContext context, INotificationManager notificationManager)
		{
			_context = context;
			_notificationManager = notificationManager;
		}

		public Task<List<Complaint>> GetAllAsync() =>
			_context.Complaints.AsNoTracking().ToListAsync();

		public Task<Complaint?> GetByIdAsync(int id) =>
			_context.Complaints.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

		public Task<List<Complaint>> GetByCitizenIdAsync(int citizenId) =>
			_context.Complaints.AsNoTracking().Where(c => c.CitizenId == citizenId).ToListAsync();

		public async Task<bool> AddAsync(Complaint complaint)
		{
			await _context.Complaints.AddAsync(complaint);
			await _context.SaveChangesAsync();
			_notificationManager.CreateForCitizen(complaint.CitizenId, $"Your complaint been added");

			return true;
		}

		public async Task<bool> UpdateAsync(Complaint updated)
		{
			var c = await _context.Complaints.FirstOrDefaultAsync(x => x.Id == updated.Id);
			if (c is null) return false;

			c.Title = updated.Title;
			c.Description = updated.Description;
			c.Location = updated.Location;
			c.ImageUrl = updated.ImageUrl;
			c.Status = updated.Status;
			c.UpdatedAt = DateTime.UtcNow;
			c.CitizenId = updated.CitizenId;
			c.AdminId = updated.AdminId;
			c.AdminNote = updated.AdminNote;

			await _context.SaveChangesAsync();
			_notificationManager.CreateForCitizen(c.CitizenId, $"Your complaint #{c.Id} been updated");
			return true;
		}

		public async Task<bool> UpdateStatusAsync(int id, ComplaintStatus status, int adminId, string? note = null)
		{
			var c = await _context.Complaints.FirstOrDefaultAsync(x => x.Id == id);
			if (c is null) return false;

			c.Status = status;
			c.AdminId = adminId;
			if (!string.IsNullOrWhiteSpace(note)) c.AdminNote = note;
			c.UpdatedAt = DateTime.UtcNow;
			await _context.SaveChangesAsync();
			_notificationManager.CreateForCitizen(c.CitizenId, $"Your complaint #{c.Id} status changed to: {status}");
			return true;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var c = await _context.Complaints.FirstOrDefaultAsync(x => x.Id == id);
			if (c is null) return false;
			_context.Complaints.Remove(c);
			await _context.SaveChangesAsync();
			return true;
		}

		public Task<List<Complaint>> GetUnresolvedAsync() =>
			_context.Complaints.AsNoTracking()
				.Where(c => c.Status == ComplaintStatus.Pending || c.Status == ComplaintStatus.InProgress)
				.ToListAsync();
	}
}
