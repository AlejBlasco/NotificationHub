using Azure.Messaging.ServiceBus;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NotificationHub.Application.Configuration.Models;
using System.Threading;

namespace NotificationHub.Application.QueueHandlers;

public class AzureServiceBusHandler : IQueueHandler
{
    private readonly AzureServiceBusConfiguration _configuration;
    private readonly ILogger _logger;

    internal AzureServiceBusHandler(AzureServiceBusConfiguration configuration, ILogger logger)
    {
        _configuration = configuration
            ?? throw new ArgumentNullException(nameof(configuration));

        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> CountActiveMessagesInQueue(CancellationToken cancellationToken = default)
    {
        int count = 0;

        try
        {
            var validator = new AzureServiceBusConfigurationValidator();
            validator.ValidateAndThrow(_configuration);

            await using var client = new ServiceBusClient(_configuration.ConnectionString, new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });

            // Create reader
            ServiceBusReceiver receiver = client.CreateReceiver(_configuration.QueueName);

            // Read
            var receivedMessages = receiver.ReceiveMessagesAsync(_configuration.MaxMessagesPerBatch, _configuration.MaxWaitTime, cancellationToken);
            count = receivedMessages.Result
                .Count();
            await receiver.DisposeAsync();

            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<Dictionary<string, string>> ReadMessagesFromQueue(bool markAsProccessed = true, CancellationToken cancellationToken = default)
    {
        Dictionary<string, string> messages = new Dictionary<string, string>();

        try
        {
            await using var client = new ServiceBusClient(_configuration.ConnectionString, new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });

            // Set options and create reader
            ServiceBusReceiver receiver = client.CreateReceiver(queueName: _configuration.QueueName,
                options: new ServiceBusReceiverOptions
                {
                    ReceiveMode = markAsProccessed
                        ? ServiceBusReceiveMode.ReceiveAndDelete
                        : ServiceBusReceiveMode.PeekLock
                });

            // Read
            var receivedMessages = receiver.ReceiveMessagesAsync(_configuration.MaxMessagesPerBatch, _configuration.MaxWaitTime, cancellationToken);
            foreach (ServiceBusReceivedMessage receivedMessage in receivedMessages.Result)
            {
                messages.Add(key: receivedMessage.MessageId,
                    value: receivedMessage.Body.ToString());
            }

            return messages;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            messages.Clear();
            throw;
        }
    }

    public async Task<bool> AddMessageToQueue(string jsonMessage, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var client = new ServiceBusClient(_configuration.ConnectionString, new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });

            // Create the sender and the message
            ServiceBusSender sender = client.CreateSender(_configuration.QueueName);
            ServiceBusMessage message = new ServiceBusMessage(jsonMessage);

            // Send the message
            await sender.SendMessageAsync(message, cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
