using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationHub.Application.Configuration.Models;
using NotificationHub.Application.Senders;
using System.Text.Json;

namespace NotificationHub.Application.Test.Sender;

public class SmsAzureCommsSenderTests
{
    private readonly Mock<ILogger> _logger;

    public SmsAzureCommsSenderTests()
    {
        _logger = new Mock<ILogger>(MockBehavior.Loose);
    }

    [Fact(Skip = "Do not run until params can be securetly stored")]
    public async Task Send_ShouldReturnOk_IfAllParamsCorrect()
    {
        // Arrange
        var jsonConfig = "{\"CommunicationServiceConnectionString\":\"CommunicationServiceConnectionString\",\"PhoneFrom\":\"PhoneFrom\"}";
        var configuration = JsonSerializer.Deserialize<SmsAzureConfiguration>(jsonConfig);

        Senders.ISender sender = new SmsAzureCommsSender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("+34000000000", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfToIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"CommunicationServiceConnectionString\":\"CommunicationServiceConnectionString\",\"PhoneFrom\":\"PhoneFrom\"}";
        var configuration = JsonSerializer.Deserialize<SmsAzureConfiguration>(jsonConfig);

        Senders.ISender sender = new SmsAzureCommsSender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfBodyIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"CommunicationServiceConnectionString\":\"CommunicationServiceConnectionString\",\"PhoneFrom\":\"PhoneFrom\"}";
        var configuration = JsonSerializer.Deserialize<SmsAzureConfiguration>(jsonConfig);

        Senders.ISender sender = new SmsAzureCommsSender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("+34000000000", "subject", "", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfCommunicationServiceConnectionStringIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"CommunicationServiceConnectionString\":\"\",\"PhoneFrom\":\"PhoneFrom\"}";
        var configuration = JsonSerializer.Deserialize<SmsAzureConfiguration>(jsonConfig);

        Senders.ISender sender = new SmsAzureCommsSender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("+34000000000", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfPhoneFromIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"CommunicationServiceConnectionString\":\"CommunicationServiceConnectionString\",\"PhoneFrom\":\"\"}";
        var configuration = JsonSerializer.Deserialize<SmsAzureConfiguration>(jsonConfig);

        Senders.ISender sender = new SmsAzureCommsSender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("+34000000000", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}
