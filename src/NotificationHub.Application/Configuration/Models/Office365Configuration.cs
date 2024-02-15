using FluentValidation;

namespace NotificationHub.Application.Configuration.Models;

public class Office365Configuration
{

}

internal class Office365ConfigurationValidator
    : AbstractValidator<Office365Configuration>
{
    internal Office365ConfigurationValidator() { }
}
