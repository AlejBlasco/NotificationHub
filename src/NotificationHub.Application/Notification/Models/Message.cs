namespace NotificationHub.Application.Notification.Models;

public class Message
{
    public string? Body { get; set; }

    public string? Subject { get; set; }

    public string? To { get; set; }

    private Message() { }
    public Message(string? body, string? subject, string? to)
    {
        Body = body;
        Subject = subject;
        To = to;
    }
}
