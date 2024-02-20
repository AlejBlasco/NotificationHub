using Azure.Communication.Sms;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NotificationHub.Application.Configuration.Models;
using System.ComponentModel.DataAnnotations;

namespace NotificationHub.Application.Senders;

public class SmsAzureCommsSender : ISender
{
    private readonly SmsAzureConfiguration _config;
    private readonly ILogger _logger;

    public SmsAzureCommsSender(SmsAzureConfiguration configuration, ILogger logger)
    {
        _config = configuration
            ?? throw new ArgumentNullException(nameof(configuration));

        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SendAsync(string? to, string? subject, string? body, CancellationToken cancellationToken)
    {
        try
        {
            // Config validations
            SmsAzureConfigurationValidator validator = new SmsAzureConfigurationValidator();
            validator.ValidateAndThrow(_config);

            // Params validations
            if (string.IsNullOrWhiteSpace(to))
                throw new FluentValidation.ValidationException("To must not be null or empty");
            else
            {
                var mailValidator = new PhoneAttribute();
                if (!mailValidator.IsValid(to))
                    throw new FluentValidation.ValidationException("To must be a valid phone number");
            }

            if (string.IsNullOrWhiteSpace(body))
                throw new FluentValidation.ValidationException("Body must not be null or empty");

            SmsClient client = new SmsClient(_config.CommunicationServiceConnectionString);
            await client.SendAsync(_config.PhoneFrom, to, body, new SmsSendOptions(false)
            {
                Tag = subject
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            await Task.FromException(ex);
        }
    }
}
