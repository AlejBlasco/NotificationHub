using FluentAssertions;
using NotificationHub.Application.Configuration.Models;
using System.Text.Json;

namespace NotificationHub.Application.Test.Configuration;

public class SmsAzureConfigurationTests
{
    [Theory]
    [InlineData("{\"CommunicationServiceConnectionString\":\"CommunicationServiceConnectionString\",\"PhoneFrom\":\"PhoneFrom\"}")]
    public async Task Validator_ShouldReturnOk_IfJsonIsCorrect(string jsonConfig)
    {
        // Arrange
        var instance = JsonSerializer.Deserialize<SmsAzureConfiguration>(jsonConfig);
        var validator = new SmsAzureConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validator_ShouldThrowException_IfConnectionStringIsNullOrEmpty()
    {
        // Arrange
        string jsonConfig = "{\"CommunicationServiceConnectionString\":\"\",\"PhoneFrom\":\"PhoneFrom\"}";
        var instance = JsonSerializer.Deserialize<SmsAzureConfiguration>(jsonConfig);
        var validator = new SmsAzureConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldThrowException_IfPhoneFromIsNullOrEmpty()
    {
        // Arrange
        string jsonConfig = "{\"CommunicationServiceConnectionString\":\"CommunicationServiceConnectionString\",\"PhoneFrom\":\"\"}";
        var instance = JsonSerializer.Deserialize<SmsAzureConfiguration>(jsonConfig);
        var validator = new SmsAzureConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }
}
