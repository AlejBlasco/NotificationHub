using System.Text.Json.Serialization;

namespace NotificationHub.Application.QueueHandlers;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QueueHandlerType
{
    Azure_ServiceBus = 1
}
