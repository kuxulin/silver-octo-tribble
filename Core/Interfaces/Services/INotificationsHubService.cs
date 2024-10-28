using Core.DTOs.Change;

namespace Core.Interfaces.Services;
public interface INotificationsHubService
{
    Task OnChangeCreated(ChangeReadDTO id);
}
