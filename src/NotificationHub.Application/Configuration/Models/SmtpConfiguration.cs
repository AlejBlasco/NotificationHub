using FluentValidation;

namespace NotificationHub.Application.Configuration.Models;

public class SmtpConfiguration
{
    private readonly string emailFrom;
    private readonly string user;
    private readonly string password;
    private readonly string server;
    private readonly int serverPort;
    private readonly bool enableSSL;
    private readonly bool enableHtmlBody;

    public string EmailFrom { get { return emailFrom; } }
    public string User { get { return user; } }
    public string Password { get { return password; } }
    public string Server { get { return server; } }
    public int ServerPort { get { return serverPort; } }
    public bool EnableSSL { get { return enableSSL; } }
    public bool EnableHtmlBody { get { return enableHtmlBody; } }

    public SmtpConfiguration()
    {
        emailFrom = "";
        user = "";
        password = "";
        server = "";
        serverPort = 0;
        enableSSL = false;
        enableHtmlBody = true;
    }
}

internal class SmtpConfigurationValidator
    : AbstractValidator<SmtpConfiguration>
{
    internal SmtpConfigurationValidator()
    {
        RuleFor(r => r.EmailFrom)
            .NotNull()
            .NotEmpty()
            .WithMessage("EmailFrom can not be null or empty");

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