using MediatR;
using Microsoft.Extensions.Logging;
using NotificationHub.Application.Configuration.Models;
using NotificationHub.Application.Configuration.Queries;
using System.Text.Json;

namespace NotificationHub.Application.QueueHandlers;

public static class QueueHandlerFactory
{
    public static IQueueHandler? GetHandler(QueueHandlerType type, ISender mediator, ILogger logger)
    {
        IQueueHandler? queueHandler = null;

        var jsonConfig = mediator.Send(new GetQueueHandlerConfigurationByTypeQuery(type))
            .Result;

        if (!string.IsNullOrWhiteSpace(jsonConfig))
        {
            switch (type)
            {
                case QueueHandlerType.Azure_ServiceBus:
                    queueHandler = new AzureServiceBusHandler(configuration: JsonSerializer.Deserialize<AzureServiceBusConfiguration>(jsonConfig)!,
                        logger: logger);
                    break;
            }
        }

        return queueHandler;
    }
}
