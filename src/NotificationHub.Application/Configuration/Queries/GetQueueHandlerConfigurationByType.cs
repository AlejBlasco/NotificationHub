using MediatR;
using Microsoft.Extensions.Configuration;
using NotificationHub.Application.QueueHandlers;

namespace NotificationHub.Application.Configuration.Queries
{
    public record GetQueueHandlerConfigurationByTypeQuery(QueueHandlerType Type)
        : IRequest<string>;

    public class GetQueueHandlerConfigurationByTypeQueryHandler
        : IRequestHandler<GetQueueHandlerConfigurationByTypeQuery, string>
    {
        private readonly IConfiguration _configuration;

        public GetQueueHandlerConfigurationByTypeQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string> Handle(GetQueueHandlerConfigurationByTypeQuery query, CancellationToken cancellationToken)
        {
            var jsonConfig = string.Empty;

            await Task.Run(() =>
            {
                if (query.Type == QueueHandlerType.Azure_ServiceBus)
                    jsonConfig = _configuration["Configurations:AzureServiceBus"];
            }, cancellationToken);

            return jsonConfig ?? string.Empty;
        }
    }
}
