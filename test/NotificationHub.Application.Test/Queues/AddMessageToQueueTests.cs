using FluentAssertions;
using Moq;
using NotificationHub.Application.QueueHandlers;
using NotificationHub.Application.Queues.Commands;

namespace NotificationHub.Application.Test.Queues;

public class AddMessageToQueueTests
{
    [Fact]
    public async Task Add_ShouldReturnOk_IfAllParamsAreCorrect()
    {
        // Arrange
        var jsonMessage = "{\"a\":\"a\",\"v\":\"v\"}";

        var queueHandler = new Mock<IQueueHandler>();
        queueHandler.Setup(x => x.AddMessageToQueue(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        // Act
        var handler = new AddMessageToQueueCommandHandler();
        var command = new AddMessageToQueueCommand(queueHandler.Object, jsonMessage);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.GetType().Should().Be(typeof(bool));
    }

    [Fact]
    public async Task Validator_ShouldReturnOk_IfAllParamsAreCorrect()
    {
        // Arrange
        var jsonMessage = "{\"a\":\"a\",\"v\":\"v\"}";

        var queueHandler = new Mock<IQueueHandler>();
        queueHandler.Setup(x => x.AddMessageToQueue(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        // Act
        var validator = new AddMessageToQueueCommandValidator();
        var command = new AddMessageToQueueCommand(queueHandler.Object, jsonMessage);
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validator_ShouldReturnNOk_IfHandlerIsNull()
    {
        // Arrange
        var jsonMessage = "{\"a\":\"a\",\"v\":\"v\"}";

        // Act
        var validator = new AddMessageToQueueCommandValidator();
        var command = new AddMessageToQueueCommand(null, jsonMessage);
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors.Select(x => x.PropertyName).Should().Contain(nameof(command.QueueHandler));
    }

    [Fact]
    public async Task Validator_ShouldReturnNOk_IfJsonMessageIsNull()
    {
        // Arrange
        var queueHandler = new Mock<IQueueHandler>();
        queueHandler.Setup(x => x.AddMessageToQueue(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        // Act
        var validator = new AddMessageToQueueCommandValidator();
        var command = new AddMessageToQueueCommand(queueHandler.Object, null);
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors.Select(x => x.PropertyName).Should().Contain(nameof(command.jsonMessage));
    }
}
