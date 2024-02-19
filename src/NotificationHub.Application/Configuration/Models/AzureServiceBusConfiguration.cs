using FluentValidation;

namespace NotificationHub.Application.Configuration.Models;

public class AzureServiceBusConfiguration
{
    public string? ConnectionString { get; set; }
    public string? QueueName { get; set; }
    public int MaxMessagesPerBatch { get; set; } = 10;
    public TimeSpan MaxWaitTime { get; set; } = new TimeSpan(hours: 0, minutes: 0, seconds: 2);
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

        RuleFor(r => r.MaxMessagesPerBatch)
            .GreaterThan(0)
            .WithMessage("MaxMessagesPerBatch can not be zero");

        RuleFor(r => r.MaxWaitTime)
            .GreaterThan(new TimeSpan(hours: 0, minutes: 0, seconds: 0))
            .WithMessage("MaxWaitTime can not be zero");
    }
}
