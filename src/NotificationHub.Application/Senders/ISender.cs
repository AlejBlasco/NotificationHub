namespace NotificationHub.Application.Senders;

public interface ISender
{
    Task SendAsync(string to, string? subject, string body, CancellationToken cancellationToken);
}
