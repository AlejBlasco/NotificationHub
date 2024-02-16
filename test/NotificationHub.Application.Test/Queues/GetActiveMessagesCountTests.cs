using FluentAssertions;
using Moq;
using NotificationHub.Application.QueueHandlers;
using NotificationHub.Application.Queues.Queries;

namespace NotificationHub.Application.Test.Queues;

public class GetActiveMessagesCountTests
{
    [Fact]
    public async Task Get_ShouldReturnOk_IfAllParamsAreCorrect()
    {
        // Arrange
        var queueHandler = new Mock<IQueueHandler>();
        queueHandler.Setup(x => x.CountActiveMessagesInQueue(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        // Act
        var handler = new GetActiveMessagesCountQueryHandler();
        var query = new GetActiveMessagesCountQuery(queueHandler.Object);
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.GetType().Should().Be(typeof(int));
    }

    [Fact]
    public async Task Validator_ShouldReturnOk_IfAllParamsAreCorrect()
    {
        // Arrange
        var queueHandler = new Mock<IQueueHandler>();
        queueHandler.Setup(x => x.CountActiveMessagesInQueue(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        // Act
        var validator = new GetActiveMessagesCountQueryValidator();
        var query = new GetActiveMessagesCountQuery(queueHandler.Object);
        var result = await validator.ValidateAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validator_ShouldReturnNOk_IfHandlerIsNull()
    {
        // Arrange

        // Act
        var validator = new GetActiveMessagesCountQueryValidator();
        var query = new GetActiveMessagesCountQuery(null);
        var result = await validator.ValidateAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors.Select(x => x.PropertyName).Should().Contain(nameof(query.QueueHandler));
    }
}
