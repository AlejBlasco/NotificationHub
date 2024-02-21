using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationHub.Application.Configuration.Models;
using NotificationHub.Application.Senders;
using System.Text.Json;

namespace NotificationHub.Application.Test.Sender;

public class EmailSmtpSenderTests
{
    private readonly Mock<ILogger> _logger;

    public EmailSmtpSenderTests()
    {
        _logger = new Mock<ILogger>(MockBehavior.Loose);
    }

    [Fact(Skip = "Do not run until params can be securetly stored")]
    public async Task Send_ShouldReturnOk_IfAllParamsCorrect()
    {
        // Arrange
        var jsonConfig = "{\"EmailFrom\":\"a@a.com\",\"User\":\"b\",\"Password\":\"c\",\"Server\":\"d\",\"ServerPort\":1,\"EnableSSL\":false,\"EnableHtmlBody\":false}";
        var configuration = JsonSerializer.Deserialize<SmtpConfiguration>(jsonConfig);

        Senders.ISender sender = new EmailSmtpSender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfEmailFromIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"EmailFrom\":\"\",\"User\":\"b\",\"Password\":\"c\",\"Server\":\"d\",\"ServerPort\":1,\"EnableSSL\":false,\"EnableHtmlBody\":false}";
        var configuration = JsonSerializer.Deserialize<SmtpConfiguration>(jsonConfig);

        Senders.ISender sender = new EmailSmtpSender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfEmailFromNotAnEmail()
    {
        // Arrange
        var jsonConfig = "{\"EmailFrom\":\"aaaaa\",\"User\":\"b\",\"Password\":\"c\",\"Server\":\"d\",\"ServerPort\":1,\"EnableSSL\":false,\"EnableHtmlBody\":false}";
        var configuration = JsonSerializer.Deserialize<SmtpConfiguration>(jsonConfig);

        Senders.ISender sender = new EmailSmtpSender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfUserIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"EmailFrom\":\"aaaaa\",\"User\":\"\",\"Password\":\"c\",\"Server\":\"d\",\"ServerPort\":1,\"EnableSSL\":false,\"EnableHtmlBody\":false}";
        var configuration = JsonSerializer.Deserialize<SmtpConfiguration>(jsonConfig);

        Senders.ISender sender = new EmailSmtpSender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfPasswordIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"EmailFrom\":\"aaaaa\",\"User\":\"b\",\"Password\":\"\",\"Server\":\"d\",\"ServerPort\":1,\"EnableSSL\":false,\"EnableHtmlBody\":false}";
        var configuration = JsonSerializer.Deserialize<SmtpConfiguration>(jsonConfig);

        Senders.ISender sender = new EmailSmtpSender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfServerIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"EmailFrom\":\"a@a.com\",\"User\":\"b\",\"Password\":\"c\",\"Server\":\"\",\"ServerPort\":1,\"EnableSSL\":false,\"EnableHtmlBody\":false}";
        var configuration = JsonSerializer.Deserialize<SmtpConfiguration>(jsonConfig);

        Senders.ISender sender = new EmailSmtpSender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfToIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"EmailFrom\":\"a@a.com\",\"User\":\"b\",\"Password\":\"c\",\"Server\":\"d\",\"ServerPort\":1,\"EnableSSL\":false,\"EnableHtmlBody\":false}";
        var configuration = JsonSerializer.Deserialize<SmtpConfiguration>(jsonConfig);

        Senders.ISender sender = new EmailSmtpSender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowTaskObjectDisposedException_IfCancelled()
    {
        // Arrange
        var jsonConfig = "{\"EmailFrom\":\"a@a.com\",\"User\":\"b\",\"Password\":\"c\",\"Server\":\"d\",\"ServerPort\":1,\"EnableSSL\":false,\"EnableHtmlBody\":false}";
        var configuration = JsonSerializer.Deserialize<SmtpConfiguration>(jsonConfig);

        Senders.ISender sender = new EmailSmtpSender(configuration!, _logger.Object);

        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", cancellationTokenSource.Token);
        };
        cancellationTokenSource.Dispose();

        // Assert
        await act.Should().ThrowAsync<ObjectDisposedException>();
    }


}
