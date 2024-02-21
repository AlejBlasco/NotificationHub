using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationHub.Application.Configuration.Models;
using NotificationHub.Application.Senders;
using System.Text.Json;

namespace NotificationHub.Application.Test.Sender;

public class EmailOffice365SenderTests
{
    private readonly Mock<ILogger> _logger;

    public EmailOffice365SenderTests()
    {
        _logger = new Mock<ILogger>(MockBehavior.Loose);
    }

    [Fact(Skip = "Do not run until params can be securetly stored")]
    public async Task Send_ShouldReturnOk_IfAllParamsCorrect()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom@test.com\"}";
        Office365Configuration? configuration = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);

        Senders.ISender sender = new EmailOffice365Sender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfToIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom@test.com\"}";
        Office365Configuration? configuration = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);

        Senders.ISender sender = new EmailOffice365Sender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfToIsNotEmail()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom@test.com\"}";
        Office365Configuration? configuration = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);

        Senders.ISender sender = new EmailOffice365Sender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfBodyIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom@test.com\"}"; Office365Configuration? configuration = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);

        Senders.ISender sender = new EmailOffice365Sender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfAuthTenantIdIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom@test.com\"}"; Office365Configuration? configuration = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);

        Senders.ISender sender = new EmailOffice365Sender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfAuthClientSecretIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom@test.com\"}"; Office365Configuration? configuration = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);

        Senders.ISender sender = new EmailOffice365Sender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfAuthorityUrlBaseIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom@test.com\"}"; Office365Configuration? configuration = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);

        Senders.ISender sender = new EmailOffice365Sender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfGraphClientSecretIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom@test.com\"}"; Office365Configuration? configuration = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);

        Senders.ISender sender = new EmailOffice365Sender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfGraphApiEndpointIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"\",\"EmailFrom\": \"EmailFrom@test.com\"}"; Office365Configuration? configuration = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);

        Senders.ISender sender = new EmailOffice365Sender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<System.ArgumentException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfGraphApiEndpointIsNotUriEmpty()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom@test.com\"}"; Office365Configuration? configuration = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);

        Senders.ISender sender = new EmailOffice365Sender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<System.ArgumentException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfEmailFromIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"\"}"; Office365Configuration? configuration = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);

        Senders.ISender sender = new EmailOffice365Sender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowValidationException_IfEmailIsNotEmail()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom\"}"; Office365Configuration? configuration = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);

        Senders.ISender sender = new EmailOffice365Sender(configuration!, _logger.Object);

        // Act
        var act = async () =>
        {
            await sender.SendAsync("to@to.com", "subject", "body", CancellationToken.None);
        };

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Send_ShouldThrowTaskObjectDisposedException_IfCancelled()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"https://www.test.com\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom@test.com\"}"; Office365Configuration? configuration = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);

        Senders.ISender sender = new EmailOffice365Sender(configuration!, _logger.Object);

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
