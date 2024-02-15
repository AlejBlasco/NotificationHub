namespace NotificationHub.Application.Notification.Models;

internal class Message
{
    internal string Body { get; set; } = string.Empty;

    internal string? Subject { get; set; }

    internal string To { get; set; } = string.Empty;

    private Message() { }   
    internal Message(string body, string? subject, string to)
    {
        Body = body;
        Subject = subject;
        To = to;
    }
}
