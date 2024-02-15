using FluentValidation;

namespace NotificationHub.Application.Configuration.Models;

public class SmsAzureConfiguration
{
}

internal class SmsAzureConfigurationValidator
    : AbstractValidator<SmsAzureConfiguration>
{
    internal SmsAzureConfigurationValidator()
    {

    }
}
