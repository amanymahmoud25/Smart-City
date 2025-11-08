using Smart_City.Dtos;

namespace Smart_City.Managers;

public interface INotificationManager
{
    bool Create(NotificationCreateDto dto);
    bool CreateForCitizen(int citizenId, string message);
    bool Broadcast(string message);
    bool Delete(int id);

    List<NotificationDto> GetAll();
    List<NotificationDto> GetByCitizenId(int citizenId);
    List<NotificationDto> GetByCitizenName(string citizenName);

    NotificationDto GetById(int id);
}
