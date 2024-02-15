using Microsoft.Extensions.Logging;
using NotificationHub.Application.Configuration.Models;
using NotificationHub.Application.Configuration.Queries;
using System.Text.Json;

namespace NotificationHub.Application.Senders;

internal static class SenderFactory
{
    private static readonly Dictionary<NotificationType, ISender> _cache = new Dictionary<NotificationType, ISender>();

    internal static ISender? GetSender(NotificationType type, MediatR.ISender mediator, ILogger logger)
    {
        if (!_cache.TryGetValue(type, out ISender? sender))
        {
            lock (_cache)
            {
                var jsonConfig = mediator.Send(new GetSenderConfigurationByTypeQuery(type))
                    .Result;

                switch (type)
                {
                    case NotificationType.Email_SMTP:
                        sender = new EmailSmtpSender(configuration: JsonSerializer.Deserialize<SmtpConfiguration>(jsonConfig)!,
                             logger);
                        break;
                    case NotificationType.Email_O365:
                        sender = new EmailOffice365Sender(configuration: JsonSerializer.Deserialize<Office365Configuration>(jsonConfig)!,
                             logger);
                        break;
                    case NotificationType.SMS:
                        sender = new SmsAzureCommsSender(configuration: JsonSerializer.Deserialize<SmsAzureConfiguration>(jsonConfig)!,
                             logger);
                        break;
                    case NotificationType.WhatsApp:
                        throw new NotImplementedException();
                }
                if (sender != null)
                    _cache.Add(type, sender);
            }
        }
        return sender;
    }
}
