using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BlazorSignalRApp.Hubs;

[ApiController]
[Route("api/[controller]")]
public class SocketController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;

    public SocketController(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("You'll be connected to the socket server!");
    }

    [HttpPost]
    public async Task<IActionResult> SendNotification([FromBody] NotificationDto dto)
    {
        // Send the message to all connected SignalR clients
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", dto.User, dto.Message);
        return Ok("Notification sent");
    }
}

public class NotificationDto
{
    public string User { get; set; }
    public string Message { get; set; }

}
