using Core.DTOs.Change;
using Core.Interfaces.Services;
using Infrastructure.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Services;
internal class NotificationsHubService : INotificationsHubService
{
    private readonly IHubContext<NotificationsHub> _hubContext;

    public NotificationsHubService(IHubContext<NotificationsHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task OnChangeCreated(ChangeReadDTO dto)
    {
        await _hubContext.Clients.Groups(dto.ProjectId.ToString()).SendAsync("ChangeCreated", dto);

        if (dto.Task.Employee is not null)
            await _hubContext.Clients.Groups(dto.Task.Employee.Id.ToString()).SendAsync("ChangeCreated", dto);
    }
}
