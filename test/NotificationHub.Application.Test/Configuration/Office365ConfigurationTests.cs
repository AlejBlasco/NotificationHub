using FluentAssertions;
using NotificationHub.Application.Configuration.Models;
using System.Text.Json;

namespace NotificationHub.Application.Test.Configuration;

public class Office365ConfigurationTests
{
    [Theory]
    [InlineData("{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom@test.com\"}")]
    public async Task Validator_ShouldReturnOk_IfJsonIsCorrect(string jsonConfig)
    {
        // Arrange
        Office365Configuration? instance = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);
        var validator = new Office365ConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldReturnError_IfAuthTenantIdIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom\"}";
        Office365Configuration? instance = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);
        var validator = new Office365ConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldReturnError_IfAuthClientSecretIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom\"}";
        Office365Configuration? instance = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);
        var validator = new Office365ConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldReturnError_IfAuthorityUrlBaseIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom\"}";
        Office365Configuration? instance = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);
        var validator = new Office365ConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldReturnError_IfGraphClientSecretIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom\"}";
        Office365Configuration? instance = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);
        var validator = new Office365ConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldReturnError_IfGraphApiEndpointIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"\",\"EmailFrom\": \"EmailFrom\"}";
        Office365Configuration? instance = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);
        var validator = new Office365ConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldReturnError_IfEmailFromIsEmpty()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"\"}";
        Office365Configuration? instance = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);
        var validator = new Office365ConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldReturnError_IfEmailIsNotEmail()
    {
        // Arrange
        var jsonConfig = "{\"AuthClientId\": \"AuthClientId\",\"AuthTenantId\": \"AuthTenantId\",\"AuthClientSecret\": \"AuthClientSecret\",\"AuthorityUrlBase\": \"AuthorityUrlBase\",\"GraphClientSecret\": \"GraphClientSecret\",\"GraphApiEndpoint\": \"GraphApiEndpoint\",\"EmailFrom\": \"EmailFrom\"}";
        Office365Configuration? instance = JsonSerializer.Deserialize<Office365Configuration>(jsonConfig);
        var validator = new Office365ConfigurationValidator();

        // Act
        var result = await validator.ValidateAsync(instance!);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }
}
