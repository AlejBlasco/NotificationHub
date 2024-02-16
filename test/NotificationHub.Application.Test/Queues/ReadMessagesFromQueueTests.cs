using FluentAssertions;
using Moq;
using NotificationHub.Application.QueueHandlers;
using NotificationHub.Application.Queues.Queries;

namespace NotificationHub.Application.Test.Queues;

public class ReadMessagesFromQueueTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Read_ShouldReturnOk_IfAllParamsAreCorrect(bool markAsProccessed)
    {
        // Arrange
        var queueHandler = new Mock<IQueueHandler>();
        queueHandler.Setup(x => x.ReadMessagesFromQueue(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new Dictionary<string, string>()));

        // Act
        var handler = new ReadMessagesFromQueueQueryHandler();
        var query = new ReadMessagesFromQueueQuery(queueHandler.Object, markAsProccessed);
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.GetType().Should().Be(typeof(Dictionary<string, string>));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Validator_ShouldReturnOk_IfAllParamsAreCorrect(bool markAsProccessed)
    {
        // Arrange
        var queueHandler = new Mock<IQueueHandler>();
        queueHandler.Setup(x => x.ReadMessagesFromQueue(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new Dictionary<string, string>()));

        // Act
        var validator = new ReadMessagesFromQueueQueryValidator();
        var query = new ReadMessagesFromQueueQuery(queueHandler.Object, markAsProccessed);
        var result = await validator.ValidateAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Validator_ShouldReturnNOk_IfHandlerIsNull(bool markAsProccessed)
    {
        // Arrange

        // Act
        var validator = new ReadMessagesFromQueueQueryValidator();
        var query = new ReadMessagesFromQueueQuery(null, markAsProccessed);
        var result = await validator.ValidateAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors.Select(x => x.PropertyName).Should().Contain(nameof(query.QueueHandler));
    }
}
