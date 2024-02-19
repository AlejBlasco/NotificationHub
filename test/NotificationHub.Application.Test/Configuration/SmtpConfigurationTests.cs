using FluentAssertions;
using Microsoft.VisualBasic;
using NotificationHub.Application.Configuration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationHub.Application.Test.Configuration;

public class SmtpConfigurationTests
{
    [Theory]
    [InlineData("{\"EmailFrom\":\"a@a.com\",\"User\":\"b\",\"Password\":\"c\",\"Server\":\"d\",\"ServerPort\":1,\"EnableSSL\":false,\"EnableHtmlBody\":false}")]
    public async Task Validator_ShouldReturnOk_IfJsonIsCorrect(string jsonConfig)
    {
        // Arrange
        SmtpConfiguration? instance = JsonSerializer.Deserialize<SmtpConfiguration>(jsonConfig);
        var validator = new SmtpConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldReturnError_IfEmailIsEmpty()
    {
        // Arrange
        SmtpConfiguration? instance = JsonSerializer.Deserialize<SmtpConfiguration>("{\"EmailFrom\":\"\",\"User\":\"b\",\"Password\":\"c\",\"Server\":\"d\",\"ServerPort\":1,\"EnableSSL\":false,\"EnableHtmlBody\":false}");
        var validator = new SmtpConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldReturnError_IfEmailIsNotAValidEmail()
    {
        // Arrange
        SmtpConfiguration? instance = JsonSerializer.Deserialize<SmtpConfiguration>("{\"EmailFrom\":\"a\",\"User\":\"b\",\"Password\":\"c\",\"Server\":\"d\",\"ServerPort\":1,\"EnableSSL\":false,\"EnableHtmlBody\":false}");
        var validator = new SmtpConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldReturnError_IfUserlIsEmpty()
    {
        // Arrange
        SmtpConfiguration? instance = JsonSerializer.Deserialize<SmtpConfiguration>("{\"EmailFrom\":\"a@a.com\",\"User\":\"\",\"Password\":\"c\",\"Server\":\"d\",\"ServerPort\":1,\"EnableSSL\":false,\"EnableHtmlBody\":false}");
        var validator = new SmtpConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldReturnError_IfPasswordIsEmpty()
    {
        // Arrange
        SmtpConfiguration? instance = JsonSerializer.Deserialize<SmtpConfiguration>("{\"EmailFrom\":\"a@a.com\",\"User\":\"b\",\"Password\":\"\",\"Server\":\"d\",\"ServerPort\":1,\"EnableSSL\":false,\"EnableHtmlBody\":false}");
        var validator = new SmtpConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldReturnError_IfServerIsEmpty()
    {
        // Arrange
        SmtpConfiguration? instance = JsonSerializer.Deserialize<SmtpConfiguration>("{\"EmailFrom\":\"a@a.com\",\"User\":\"b\",\"Password\":\"c\",\"Server\":\"\",\"ServerPort\":1,\"EnableSSL\":false,\"EnableHtmlBody\":false}");
        var validator = new SmtpConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }
}
