using FluentValidation;

namespace NotificationHub.Application.Configuration.Models;

public class SmtpConfiguration
{
    public string? EmailFrom { get; set; }
    public string? User { get; set; }
    public string? Password { get; set; }
    public string? Server { get; set; }
    public int ServerPort { get; set; }
    public bool EnableSSL { get; set; }
    public bool EnableHtmlBody { get; set; }

}

public class SmtpConfigurationValidator
    : AbstractValidator<SmtpConfiguration>
{
    public SmtpConfigurationValidator()
    {
        RuleFor(r => r.EmailFrom)
            .NotNull()
            .NotEmpty()
            .WithMessage("EmailFrom can not be null or empty")
            .EmailAddress().WithMessage("A valid email is required");

        RuleFor(r => r.User)
            .NotNull()
            .NotEmpty()
            .WithMessage("User can not be null or empty");

        RuleFor(r => r.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password can not be null or empty");

        RuleFor(r => r.Server)
            .NotNull()
            .NotEmpty()
            .WithMessage("Server can not be null or empty");
    }
}