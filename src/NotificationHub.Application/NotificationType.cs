using System.Text.Json.Serialization;

namespace NotificationHub.Application;

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum NotificationType
{
    Email_SMTP = 1,
    Email_O365 = 2,
    SMS = 3,
    WhatsApp = 4
}
