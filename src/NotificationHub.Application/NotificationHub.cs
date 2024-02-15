using Microsoft.Extensions.Logging;
using NotificationHub.Application.Notification.Commands;
using NotificationHub.Application.Notification.Models;
using NotificationHub.Application.Senders;

namespace NotificationHub.Application;

public class NotificationHub : INotificationHub
{
    private readonly MediatR.ISender mediator;
    private readonly ILogger<NotificationHub> logger;

    public NotificationHub(MediatR.ISender mediator, ILogger<NotificationHub> logger)
    {
        this.mediator = mediator
            ?? throw new ArgumentNullException(nameof(mediator));

        this.logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SendNotification(string messageBody, string? messageSubject, string messageTo, NotificationType senderType)
    {
        var command = new SendNotificationCommand(Message: new Message(messageBody, messageSubject, messageTo),
             Sender: SenderFactory.GetSender(senderType, mediator, logger));

        await mediator.Send(command);
    }
}
