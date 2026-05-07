using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Application;

[Authorize]
public class NotificationHub : Hub
{
    
}