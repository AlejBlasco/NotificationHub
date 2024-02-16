using Azure.Messaging.ServiceBus;
using NotificationHub.Application.Configuration.Models;

namespace NotificationHub.Application.QueueHandlers;

public interface IQueueHandler
{
    Task<int> CountActiveMessagesInQueue(CancellationToken cancellationToken = default);

    Task<Dictionary<string, string>> ReadMessagesFromQueue(bool markAsProccessed = true, CancellationToken cancellationToken = default);

    Task<bool> AddMessageToQueue(string jsonMessage, CancellationToken cancellationToken = default);
}
