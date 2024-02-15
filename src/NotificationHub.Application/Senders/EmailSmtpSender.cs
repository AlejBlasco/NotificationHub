using FluentValidation;
using Microsoft.Extensions.Logging;
using NotificationHub.Application.Configuration.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

namespace NotificationHub.Application.Senders;

public class EmailSmtpSender : ISender
{
    private readonly SmtpConfiguration _config;
    private readonly ILogger _logger;

    public EmailSmtpSender(SmtpConfiguration configuration, ILogger logger)
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
            SmtpConfigurationValidator validator = new SmtpConfigurationValidator();
            validator.ValidateAndThrow(_config);

            // Params validations
            if (string.IsNullOrWhiteSpace(to))
                throw new FluentValidation.ValidationException("To must not be null or empty");
            else
            {
                var mailValidator = new EmailAddressAttribute();
                if (!mailValidator.IsValid(to))
                    throw new FluentValidation.ValidationException("To must be a valid email");
            }

            if (string.IsNullOrWhiteSpace(body))
                throw new FluentValidation.ValidationException("Body must not be null or empty");

            // Message
            var mailMessage = new MailMessage
            {
                IsBodyHtml = _config.EnableHtmlBody,
                From = new MailAddress(_config.EmailFrom!),
                Subject = subject,
                Body = body,
            };
            mailMessage.To.Add(new MailAddress(to));

            // Client
            var smtpClient = new SmtpClient(_config.Server)
            {
                Port = _config.ServerPort,
                Credentials = new NetworkCredential(_config.User, _config.Password),
                EnableSsl = _config.EnableSSL,
            };
            smtpClient.SendCompleted += (sender, e) =>
            {
                smtpClient.Dispose();
                mailMessage.Dispose();
            };

            // Send
            var result = Task.Run(() =>
            {
                smtpClient.Send(mailMessage);
            }, cancellationToken);

            if (result.IsCanceled)
                await Task.FromCanceled(cancellationToken).ConfigureAwait(false);

        }
        catch (OperationCanceledException cancelled)
        {
            _logger.LogError(cancelled.Message);
            await Task.FromException(cancelled);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            await Task.FromException(ex);
        }
    }
}
