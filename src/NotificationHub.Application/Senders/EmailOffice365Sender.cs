using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using NotificationHub.Application.Configuration.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text.Json;

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

    public async Task SendAsync(string? to, string? subject, string? body, CancellationToken cancellationToken)
    {
        try
        {
            // Config validations
            Office365ConfigurationValidator validator = new Office365ConfigurationValidator();
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

            // Manage cancelled
            if (cancellationToken.IsCancellationRequested)
                await Task.FromCanceled(cancellationToken);

            // Set up the authentication context and acquire a token
            var authBuilder = ConfidentialClientApplicationBuilder
                .Create(_config.AuthClientId)
                .WithAuthority($"{_config.AuthorityUrlBase}{_config.AuthTenantId}/v2.0")
                .WithClientSecret(_config.AuthClientSecret)
                .Build();

            var authResult = await authBuilder.AcquireTokenForClient(new[] { _config.GraphClientSecret })
                .ExecuteAsync();

            // Set up the HTTP client and add the access token to the authorization header
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);

            // Set up the email message
            var emailMessage = new
            {
                message = new
                {
                    subject = subject ?? "",
                    body = new
                    {
                        contentType = "Html",
                        content = body
                    },
                    toRecipients = new[]
                    {
                        new
                        {
                            emailAddress = new
                            {
                                address = to
                            }
                        }
                    }
                }
            };

            // Send
            var jsonMessage = JsonSerializer.Serialize(emailMessage);
            var response = await httpClient.PostAsync(
                $"{_config.GraphApiEndpoint}/users/{_config.EmailFrom}/sendMail",
                new StringContent(jsonMessage, System.Text.Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
                _logger.LogInformation($"Email sent to {to} successfully.");
            else
                _logger.LogInformation($"Failure Email to {to}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            await Task.FromException(ex);
        }
    }
}
