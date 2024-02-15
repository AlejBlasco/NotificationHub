using Microsoft.AspNetCore.Mvc;
using NotificationHub.Application;

namespace NotificationHub.API.Controllers;

public class NotificationController : BaseController
{
    private readonly INotificationHub notificationHub;

    public NotificationController(ILogger<NotificationController> logger, INotificationHub notificationHub)
        : base(logger)
    {
        this.notificationHub = notificationHub
            ?? throw new ArgumentNullException(nameof(notificationHub));
    }

    [HttpPost("SendNotification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SendNotification(NotificationType senderType, string messageTo, string messageBody, string messageSubject = "")
    {
        await notificationHub.SendNotification(messageBody, messageSubject, messageTo, senderType);
        return Ok();
    }
}
