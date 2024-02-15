using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NotificationHub.Application.Configuration.Queries;

namespace NotificationHub.Application.Test.Configuration;

public class GetSenderConfigurationByTypeTests
{
    private readonly Dictionary<string, string> _inMemorySettings = new Dictionary<string, string>
    {
        {"Configurations:Smtp", "This will be a jSON"},
    };

    [Theory]
    [InlineData(NotificationType.Email_SMTP)]
    public async Task Get_ShouldReturnString_IfTypeIsSetted(NotificationType notificationType)
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(_inMemorySettings!)
            .Build();

        // Act
        var handler = new GetSenderConfigurationByTypeQueryHandler(configuration);
        var query = new GetSenderConfigurationByTypeQuery(notificationType);
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeOfType<string>();
        result.Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData(NotificationType.Email_O365)]
    [InlineData(NotificationType.SMS)]
    [InlineData(NotificationType.WhatsApp)]
    public async Task Get_ShouldReturnStringEmpty_IfTypeIsNotSetted(NotificationType notificationType)
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(_inMemorySettings!)
            .Build();

        // Act
        var handler = new GetSenderConfigurationByTypeQueryHandler(configuration);
        var query = new GetSenderConfigurationByTypeQuery(notificationType);
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeOfType<string>();
        result.Should().BeNullOrWhiteSpace();
    }
}
