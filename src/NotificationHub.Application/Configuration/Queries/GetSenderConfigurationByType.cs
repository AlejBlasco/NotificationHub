using MediatR;
using Microsoft.Extensions.Configuration;

namespace NotificationHub.Application.Configuration.Queries;

internal record GetSenderConfigurationByTypeQuery(NotificationType Type)
    : IRequest<string>;

internal class GetSenderConfigurationByTypeQueryHandler
    : IRequestHandler<GetSenderConfigurationByTypeQuery, string>
{
    private readonly IConfiguration _configuration;

    public GetSenderConfigurationByTypeQueryHandler(IConfiguration configuration)
    {
        _configuration = configuration
            ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<string> Handle(GetSenderConfigurationByTypeQuery query, CancellationToken cancellationToken)
    {
        var jsonConfig = string.Empty;

        await Task.Run(() =>
        {
            if (query.Type == NotificationType.Email_SMTP)
                jsonConfig = _configuration["Configurations:Smtp"];
        });

        return jsonConfig ?? string.Empty;
    }
}
