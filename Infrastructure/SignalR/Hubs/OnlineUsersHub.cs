using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Infrastructure.SignalR.Hubs;
[Authorize(Policy = DefinedPolicy.DefaultPolicy)]
internal class OnlineStatusHub : Hub
{
    private static ConcurrentBag<string> _onlineUsers = [];
    public override async Task OnConnectedAsync()
    {
        var userName = Context.User?.Identity?.Name!;
        _onlineUsers.Add(userName);
        await Clients.All.SendAsync("AddOnlineUser", _onlineUsers);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userName = Context.User?.Identity?.Name;
        _onlineUsers = new ConcurrentBag<string>(_onlineUsers.Where(u => u != userName));
        await Clients.All.SendAsync("RemoveOfflineUser", _onlineUsers);
        await base.OnDisconnectedAsync(exception);
    }
}

