using FluentValidation;

namespace NotificationHub.Application.Configuration.Models;

public class AzureServiceBusConfiguration
{
    public string? ConnectionString { get; set; }
    public string? QueueName { get; set; }
}

public class AzureServiceBusConfigurationValidator
    : AbstractValidator<AzureServiceBusConfiguration>
{
    public AzureServiceBusConfigurationValidator()
    {
        RuleFor(r => r.ConnectionString)
            .NotNull()
            .NotEmpty()
            .WithMessage("ConnectionString can not be null or empty");

        RuleFor(r => r.QueueName)
            .NotNull()
            .NotEmpty()
            .WithMessage("QueueName can not be null or empty");
    }
}
