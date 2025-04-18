using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BlazorSignalRApp.Hubs;
using System.Collections.Concurrent;


[ApiController]
[Route("api/[controller]")]
public class SocketController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;
    private static ConcurrentDictionary<string, bool> userDndStates = new();
    private static bool doNotDisturb=false;

    public SocketController(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
        //doNotDisturb = false;
        Console.Write("Controller launch");
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("You'll be connected to the socket server!");
    }

    [HttpPost]
    public async Task<IActionResult> SendNotification([FromBody] NotificationDto dto)
    {

        // Vérifie si l'utilisateur est en DND
        if (doNotDisturb)
        {
            return Ok("dnd mode");
        }
        // Send the message to all connected SignalR clients
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", dto.User, dto.Message);
        return Ok("sent");
    }

    [HttpGet("dnd")]
    public IActionResult SetDoNotDisturb()
    {
        Console.WriteLine(doNotDisturb);
        doNotDisturb = !doNotDisturb;
        Console.WriteLine(doNotDisturb);
        return Ok($"dnd mode {(doNotDisturb ? "switched on" : "switched off") }");
    }
}

public class NotificationDto
{
    public string User { get; set; }
    public string Message { get; set; }

}
public class DndDto
{
    public string User { get; set; }
    public bool IsDnd { get; set; }
}

public class Dnd
{
    public bool isDnd;
}