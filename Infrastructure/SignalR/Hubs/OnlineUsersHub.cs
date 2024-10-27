using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.SignalR.Hubs;
public class OnlineStatusHub : Hub
{
    private static List<string> _onlineUsers = [];
    [Authorize(Policy = DefinedPolicy.DefaultPolicy)]
    public async Task UserConnectedAsync()
    {
        var userName = Context.User?.Identity?.Name;
        _onlineUsers.Add(userName);
        await Clients.All.SendAsync("AddOnlineUser", _onlineUsers);
    }

    [Authorize(Policy = DefinedPolicy.DefaultPolicy)]
    public async Task UserDisconnectedAsync()
    {
        var userName = Context.User?.Identity?.Name;
        _onlineUsers.Remove(userName);
        await Clients.All.SendAsync("RemoveOfflineUser", _onlineUsers);
    }
}

