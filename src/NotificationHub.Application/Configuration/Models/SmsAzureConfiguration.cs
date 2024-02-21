using FluentValidation;

namespace NotificationHub.Application.Configuration.Models;

public class SmsAzureConfiguration
{
    public string? CommunicationServiceConnectionString { get; set; }
    public string? PhoneFrom { get; set; }
}

public class SmsAzureConfigurationValidator
    : AbstractValidator<SmsAzureConfiguration>
{
    public SmsAzureConfigurationValidator()
    {
        RuleFor(r => r.CommunicationServiceConnectionString)
            .NotNull()
            .NotEmpty()
            .WithMessage("CommunicationServiceConnectionString can not be null or empty");

        RuleFor(r => r.PhoneFrom)
            .NotNull()
            .NotEmpty()
            .WithMessage("PhoneFrom can not be null or empty");
    }
}
