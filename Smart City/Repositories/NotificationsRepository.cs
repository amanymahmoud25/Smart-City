using Smart_City.Models;
using Microsoft.EntityFrameworkCore;

namespace Smart_City.Repositories;

public class NotificationRepository : INotificationsRepository
{
	private readonly SmartCityContext _context;

	public NotificationRepository(SmartCityContext context)
	{
		_context = context;
	}

	public List<Notification> GetAll()
	{
		return _context.Notifications
			.Include(n => n.Citizen)
			.OrderBy(n => n.CitizenId)
			.OrderByDescending(n => n.SentDate)
			.ToList();
	}

	public Notification GetById(int id)
	{
		if (id <= 0)
			return null;
		return _context.Notifications
			.Include(n => n.Citizen)
			.FirstOrDefault(n => n.Id == id);
	}

	public List<Notification> GetByCitizenId(int citizenId)
	{
		if (citizenId <= 0)
			return null;
		return _context.Notifications
			.Where(n => n.CitizenId == citizenId)
			.OrderByDescending(n => n.SentDate)
			.ToList();
	}

	public List<Notification> GetByCitizenName(string citizenName)
	{
		if (citizenName == null)
			return null;
		return _context.Notifications
			.Where(n => n.Citizen.Name.ToLower() == citizenName.ToLower())
			.OrderByDescending(n => n.SentDate)
			.ToList();
	}

	public bool Add(Notification notification)
	{
		if (notification == null)
			return false;

		if (string.IsNullOrEmpty(notification.Message) || notification.CitizenId <= 0)
			return false;

		notification.SentDate = DateTime.Now;

		_context.Notifications.Add(notification);
		_context.SaveChanges();
		return true;
	}

	/*public bool Update(Notification notification)
	{
		if (notification == null || notification.Id <= 0)
			return false;

		var existing = _context.Notifications.FirstOrDefault(n => n.Id == notification.Id);
		if (existing == null)
			return false;

		existing.Message = notification.Message;
		existing.SentDate = notification.SentDate;
		existing.CitizenId = notification.CitizenId;

		_context.SaveChanges();
		return true;
	}*/

	public bool Delete(int id)
	{
		if (id <= 0)
			return false;

		var notification = _context.Notifications.FirstOrDefault(n => n.Id == id);
		if (notification == null)
			return false;

		_context.Notifications.Remove(notification);
		_context.SaveChanges();
		return true;
	}
}
