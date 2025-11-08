using Smart_City.Models;

namespace Smart_City.Repositories;

public interface INotificationsRepository
{
     Notification GetById(int id);

     List<Notification> GetAll();
     List<Notification> GetByCitizenId(int citizenId);
     List<Notification> GetByCitizenName(string citizenName);

     bool Add(Notification notification);
     bool Delete(int id);
}

