using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationHub.Application.Configuration.Queries;
using NotificationHub.Application.QueueHandlers;

namespace NotificationHub.Application.Test.QueueHandlers;

public class QueueHandlerFactoryTests
{
    private readonly Mock<ILogger> _logger;

    public QueueHandlerFactoryTests()
    {
        _logger = new Mock<ILogger>(MockBehavior.Loose);
    }

    [Theory]
    [InlineData(QueueHandlerType.Azure_ServiceBus)]
    public void Factory_ShouldReturnIQueueHandler_IfParamsAreCorrectAndHandlerIsConfig(QueueHandlerType type)
    {
        // Arrange
        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetQueueHandlerConfigurationByTypeQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult("{\"ConnectionString\":\"aaaa\",\"QueueName\":\"bbb\"}"));

        // Act
        var result = QueueHandlerFactory.GetHandler(type, mediator.Object, _logger.Object);

        // Assert
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(QueueHandlerType.Azure_ServiceBus)]
    public void Factory_ShouldReturnNull_IfHandlerIsNotConfig(QueueHandlerType type)
    {
        // Arrange
        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetQueueHandlerConfigurationByTypeQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(""));

        // Act
        var result = QueueHandlerFactory.GetHandler(type, mediator.Object, _logger.Object);

        // Assert
        result.Should().BeNull();
    }
}
