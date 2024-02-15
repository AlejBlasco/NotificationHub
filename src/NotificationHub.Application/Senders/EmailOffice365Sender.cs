using FluentValidation;
using Microsoft.Extensions.Logging;
using NotificationHub.Application.Configuration.Models;

namespace NotificationHub.Application.Senders;

public class EmailOffice365Sender : ISender
{
    private readonly Office365Configuration _config;
    private readonly ILogger _logger;

    public EmailOffice365Sender(Office365Configuration configuration, ILogger logger)
    {
        _config = configuration
            ?? throw new ArgumentNullException(nameof(configuration));

        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SendAsync(string to, string? subject, string body, CancellationToken cancellationToken)
    {
        try
        {
            // Config validations
            Office365ConfigurationValidator validator = new Office365ConfigurationValidator();
            validator.ValidateAndThrow(_config);

            await Task.Run(() =>
            {
                // Manage cancelled
                if (cancellationToken.IsCancellationRequested)
                    Task.FromCanceled(cancellationToken);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            await Task.FromException(ex);
        }
    }
}
