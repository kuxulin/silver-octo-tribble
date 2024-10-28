using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.SignalR.Hubs;
[Authorize(Policy = DefinedPolicy.DefaultPolicy)]
internal class OnlineStatusHub : Hub
{
    private static List<string> _onlineUsers = [];
    public override async Task OnConnectedAsync()
    {
        var userName = Context.User?.Identity?.Name;
        _onlineUsers.Add(userName);
        await Clients.All.SendAsync("AddOnlineUser", _onlineUsers);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userName = Context.User?.Identity?.Name;
        _onlineUsers.Remove(userName);
        await Clients.All.SendAsync("RemoveOfflineUser", _onlineUsers);
        await base.OnDisconnectedAsync(exception);
    }
}

