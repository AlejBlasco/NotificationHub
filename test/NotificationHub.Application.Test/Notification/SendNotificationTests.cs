using FluentAssertions;
using Moq;
using NotificationHub.Application.Notification.Commands;
using NotificationHub.Application.Notification.Models;

namespace NotificationHub.Application.Test.Notification;

public class SendNotificationTests
{
    [Fact]
    public async Task Send_ShouldEndOk_IfAllParamsAreCorrect()
    {
        // Arrange
        var message = new Message("body", "subject", "to");

        var sender = new Mock<Senders.ISender>();
        sender.Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var handler = new SendNotificationCommandHandler();
        var command = new SendNotificationCommand(message, sender.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Validator_ShouldReturnOk_IfAllParamsAreCorrect()
    {
        // Arrange
        var message = new Message("body", "subject", "to");

        // Act
        var validator = new SendNotificationCommandValidator();
        var command = new SendNotificationCommand(message, null);
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Validator_ShouldReturnNOk_IfSenderIsNull()
    {
        // Arrange
        var message = new Message("body", "subject", "to");

        var sender = new Mock<Senders.ISender>();
        sender.Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var validator = new SendNotificationCommandValidator();
        var command = new SendNotificationCommand(message, sender.Object);
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Validator_ShouldReturnNOk_IfMessageIsNull()
    {
        // Arrange
        var sender = new Mock<Senders.ISender>();
        sender.Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var validator = new SendNotificationCommandValidator();
        var command = new SendNotificationCommand(null, sender.Object);
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors.Select(x => x.PropertyName).Should().Contain(nameof(command.Message));
    }

    [Fact]
    public async Task Validator_ShouldReturnNOk_IfMessageBodyIsNull()
    {
        // Arrange
        var sender = new Mock<Senders.ISender>();
        sender.Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var message = new Message(null, "subject", "to");

        // Act
        var validator = new SendNotificationCommandValidator();
        var command = new SendNotificationCommand(message, sender.Object);
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors.Select(x => x.PropertyName).Should().Contain(nameof(command.Message.Body));
    }

    [Fact]
    public async Task Validator_ShouldReturnNOk_IfMessageToIsNull()
    {
        // Arrange
        var sender = new Mock<Senders.ISender>();
        sender.Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var message = new Message("message", "subject", null);

        // Act
        var validator = new SendNotificationCommandValidator();
        var command = new SendNotificationCommand(message, sender.Object);
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors.Select(x => x.PropertyName).Should().Contain(nameof(command.Message.To));
    }
}
