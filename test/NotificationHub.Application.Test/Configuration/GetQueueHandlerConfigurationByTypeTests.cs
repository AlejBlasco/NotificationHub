using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NotificationHub.Application.Configuration.Queries;
using NotificationHub.Application.QueueHandlers;

namespace NotificationHub.Application.Test.Configuration;

public class GetQueueHandlerConfigurationByTypeTests
{
    [Theory]
    [InlineData(QueueHandlerType.Azure_ServiceBus)]
    public async Task Get_ShouldReturnString_IfTypeIsSetted(QueueHandlerType type)
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string>
        {
            {"Configurations:AzureServiceBus", "This will be a jSON"},
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        // Act
        var handler = new GetQueueHandlerConfigurationByTypeQueryHandler(configuration);
        var query = new GetQueueHandlerConfigurationByTypeQuery(type);
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeOfType<string>();
        result.Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData(QueueHandlerType.Azure_ServiceBus)]
    public async Task Get_ShouldReturnStringEmpty_IfTypeIsNotSetted(QueueHandlerType type)
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string>();

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        // Act
        var handler = new GetQueueHandlerConfigurationByTypeQueryHandler(configuration);
        var query = new GetQueueHandlerConfigurationByTypeQuery(type);
        var result = await handler.Handle(query, CancellationToken.None);


        // Assert
        result.Should().BeOfType<string>();
        result.Should().BeNullOrWhiteSpace();
    }
}
