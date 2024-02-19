using FluentAssertions;
using NotificationHub.Application.Configuration.Models;
using System.Text.Json;

namespace NotificationHub.Application.Test.Configurations;

public class AzureServiceBusConfigurationTests
{
    [Theory]
    [InlineData("{\"ConnectionString\":\"aaaa\",\"QueueName\":\"bbb\", \"MaxMessagesPerBatch\": 10, \"MaxWaitTime\":\"00:00:02\"}")]
    public async Task Validator_ShouldReturnOk_IfJsonIsCorrect(string jsonConfig)
    {
        // Arrange
        AzureServiceBusConfiguration? instance = JsonSerializer.Deserialize<AzureServiceBusConfiguration>(jsonConfig);
        var validator = new AzureServiceBusConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validator_ShouldThrowException_IfNoConnectionString()
    {
        // Arrange
        AzureServiceBusConfiguration? instance = JsonSerializer.Deserialize<AzureServiceBusConfiguration>
            ("{\"ConnectionString\":\"\",\"QueueName\":\"bbb\", \"MaxMessagesPerBatch\": 10, \"MaxWaitTime\":\"00:00:02\"}");
        var validator = new AzureServiceBusConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldTrhowException_IfNoQueueName()
    {
        // Arrange
        AzureServiceBusConfiguration? instance = JsonSerializer.Deserialize<AzureServiceBusConfiguration>
            ("{\"ConnectionString\":\"aaaa\",\"QueueName\":\"\", \"MaxMessagesPerBatch\": 10, \"MaxWaitTime\":\"00:00:02\"}");
        var validator = new AzureServiceBusConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData("{\"ConnectionString\":\"aaaa\",\"QueueName\":\"bbb\", \"MaxMessagesPerBatch\": 0, \"MaxWaitTime\":\"00:00:02\"}")]
    [InlineData("{\"ConnectionString\":\"aaaa\",\"QueueName\":\"bbb\", \"MaxMessagesPerBatch\": -1, \"MaxWaitTime\":\"00:00:02\"}")]
    public async Task Validator_ShouldTrhowException_IfMaxMessagesLowerOrEqualZero(string jsonConfig)
    {
        // Arrange
        AzureServiceBusConfiguration? instance = JsonSerializer.Deserialize<AzureServiceBusConfiguration>(jsonConfig);
        var validator = new AzureServiceBusConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldTrhowException_IfMaxWaitTimeLowerEqualZero()
    {
        // Arrange
        AzureServiceBusConfiguration? instance = JsonSerializer.Deserialize<AzureServiceBusConfiguration>
            ("{\"ConnectionString\":\"aaaa\",\"QueueName\":\"bbb\", \"MaxMessagesPerBatch\": 10, \"MaxWaitTime\":\"00:00:00\"}");
        var validator = new AzureServiceBusConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }
}
