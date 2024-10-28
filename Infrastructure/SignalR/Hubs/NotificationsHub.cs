using Core.Constants;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SignalR.Hubs;
[Authorize(Policy = DefinedPolicy.DefaultPolicy)]
internal class NotificationsHub : Hub
{
    private readonly IUserRepository _userRepository;
    private readonly IChangeService _changeService;

    public NotificationsHub(IUserRepository userRepository, IChangeService changeService)
    {
        _userRepository = userRepository;
        _changeService = changeService;
    }

    public override async Task OnConnectedAsync()
    {
        int id = int.Parse(Context.User?.Claims.First(c => c.Type == DefinedClaim.Id).Value);
        var groups = await CreateAppropriateGroups(id);
        foreach (var group in groups)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        await base.OnConnectedAsync();
    }

    private async Task<List<string>> CreateAppropriateGroups(int id)
    {
        var user = await _userRepository.GetAll()
            .Include(u => u.Manager)
            .ThenInclude(m => m.Projects)
            .FirstAsync(u => u.Id == id);
        var groups = new List<string>();

        if (user.ManagerId is not null && user.Manager.Projects is not null)
        {
            foreach (var project in user.Manager.Projects)
                groups.Add(project.Id.ToString());
        }

        if (user.EmployeeId is not null)
            groups.Add(user.EmployeeId.ToString());

        return groups;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        int id = int.Parse(Context.User?.Claims.First(c => c.Type == DefinedClaim.Id).Value);
        var groups = await CreateAppropriateGroups(id);

        foreach (var group in groups)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
