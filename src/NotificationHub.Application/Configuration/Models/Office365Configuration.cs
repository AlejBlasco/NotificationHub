using FluentValidation;

namespace NotificationHub.Application.Configuration.Models;

public class Office365Configuration
{
    public string? AuthClientId { get; set; }
    public string? AuthTenantId { get; set; }
    public string? AuthClientSecret { get; set; }

    public string? AuthorityUrlBase { get; set; }

    public string? GraphClientSecret { get; set; }
    public string? GraphApiEndpoint { get; set; }

    public string? EmailFrom { get; set; }
}

public class Office365ConfigurationValidator
    : AbstractValidator<Office365Configuration>
{
    public Office365ConfigurationValidator()
    {
        RuleFor(r => r.EmailFrom)
            .NotNull()
            .NotEmpty()
            .WithMessage("EmailFrom can not be null or empty")
            .EmailAddress().WithMessage("A valid email is required");

        RuleFor(r => r.AuthClientId)
            .NotNull()
            .NotEmpty()
            .WithMessage("AuthClientId can not be null or empty");

        RuleFor(r => r.AuthTenantId)
            .NotNull()
            .NotEmpty()
            .WithMessage("AuthTenantId can not be null or empty");

        RuleFor(r => r.AuthClientSecret)
            .NotNull()
            .NotEmpty()
            .WithMessage("AuthClientSecret can not be null or empty");

        RuleFor(r => r.AuthorityUrlBase)
            .NotNull()
            .NotEmpty()
            .WithMessage("AuthorityUrlBase can not be null or empty");

        RuleFor(r => r.GraphClientSecret)
            .NotNull()
            .NotEmpty()
            .WithMessage("GraphClientSecret can not be null or empty");

        RuleFor(r => r.EmailFrom)
            .NotNull()
            .NotEmpty()
            .WithMessage("EmailFrom can not be null or empty");

    }
}
