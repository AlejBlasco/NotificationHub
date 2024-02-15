namespace NotificationHub.Application;

public interface INotificationHub
{
    Task SendNotification(string messageBody, string? messageSubject, string messageTo, NotificationType senderType);
}
