using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationHub.Application.Configuration.Queries;
using NotificationHub.Application.QueueHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Application.Test.QueueHandlers;

public class AzureServiceBusHandlerTests
{
    private readonly Mock<ILogger> _logger;

    public AzureServiceBusHandlerTests()
    {
        _logger = new Mock<ILogger>(MockBehavior.Loose);
    }

    [Theory(Skip = "Do not run until params can be securetly stored")]
    [InlineData(QueueHandlerType.Azure_ServiceBus)]
    public async Task CountActiveMessagesInQueue_ShouldReturnNumber_IfConnect(QueueHandlerType type)
    {
        // Arrange
        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetQueueHandlerConfigurationByTypeQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(""));

        IQueueHandler? handler = QueueHandlerFactory.GetHandler(type, mediator.Object ,_logger.Object);

        // Act
        int? result = null;
        if (handler != null)
            result = await handler!.CountActiveMessagesInQueue(CancellationToken.None);

        // Assert
        handler.Should().NotBeNull();
        result.Should().NotBeNull();
        if (result != null)
            result.GetType().Should().Be(typeof(int));
    }

    [Theory(Skip = "Do not run until params can be securetly stored")]
    [InlineData(QueueHandlerType.Azure_ServiceBus)]
    public async Task ReadMessagesFromQueue_ShouldReturnNumber_IfConnect(QueueHandlerType type)
    {
        // Arrange
        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetQueueHandlerConfigurationByTypeQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(""));

        IQueueHandler? handler = QueueHandlerFactory.GetHandler(type, mediator.Object, _logger.Object);

        // Act
        Dictionary<string,string>? result = null;
        if (handler != null)
            result = await handler!.ReadMessagesFromQueue(false, CancellationToken.None);

        // Assert
        handler.Should().NotBeNull();
        result.Should().NotBeNull();
        if (result != null)
            result.GetType().Should().Be(typeof(Dictionary<string, string>));
    }

    [Theory(Skip = "Do not run until params can be securetly stored")]
    [InlineData(QueueHandlerType.Azure_ServiceBus)]
    public async Task AddMessageToQueue_ShouldReturnNumber_IfConnect(QueueHandlerType type)
    {
        // Arrange
        var dummyJson = "{\"ConnectionString\":\"aaaa\",\"QueueName\":\"bbb\"}";

        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetQueueHandlerConfigurationByTypeQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(""));

        IQueueHandler? handler = QueueHandlerFactory.GetHandler(type, mediator.Object, _logger.Object);

        // Act
        bool? result = null;
        if (handler != null)
            result = await handler!.AddMessageToQueue(dummyJson, CancellationToken.None);

        // Assert
        handler.Should().NotBeNull();
        result.Should().NotBeNull();
        if (result != null)
            result.GetType().Should().Be(typeof(bool));
    }

    [Theory]
    [InlineData(QueueHandlerType.Azure_ServiceBus)]
    public async Task CountActiveMessagesInQueue_ShouldThrowException_IfNotConnect(QueueHandlerType type)
    {
        // Arrange
        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetQueueHandlerConfigurationByTypeQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult("{\"ConnectionString\":\"aaaa\",\"QueueName\":\"bbb\"}"));

        IQueueHandler? handler = QueueHandlerFactory.GetHandler(type, mediator.Object, _logger.Object);

        // Act
        Func<Task>? act = null;
        if (handler != null)
        {
            act = async () =>
            {
                await handler!.CountActiveMessagesInQueue(CancellationToken.None);
            };
        }


        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Theory]
    [InlineData(QueueHandlerType.Azure_ServiceBus)]
    public async Task ReadMessagesFromQueue_ShouldThrowException_IfNotConnect(QueueHandlerType type)
    {
        // Arrange
        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetQueueHandlerConfigurationByTypeQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult("{\"ConnectionString\":\"aaaa\",\"QueueName\":\"bbb\"}"));

        IQueueHandler? handler = QueueHandlerFactory.GetHandler(type, mediator.Object, _logger.Object);

        // Act
        Func<Task>? act = null;
        if (handler != null)
        {
            act = async () =>
            {
                await handler!.ReadMessagesFromQueue(false, CancellationToken.None);
            };
        }


        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Theory]
    [InlineData(QueueHandlerType.Azure_ServiceBus)]
    public async Task AddMessageToQueue_ShouldThrowException_IfNotConnect(QueueHandlerType type)
    {
        // Arrange
        var dummyJson = "{\"ConnectionString\":\"aaaa\",\"QueueName\":\"bbb\"}";

        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetQueueHandlerConfigurationByTypeQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult("{\"ConnectionString\":\"aaaa\",\"QueueName\":\"bbb\"}"));

        IQueueHandler? handler = QueueHandlerFactory.GetHandler(type, mediator.Object, _logger.Object);

        // Act
        Func<Task>? act = null;
        if (handler != null)
        {
            act = async () =>
            {
                await handler!.AddMessageToQueue(dummyJson, CancellationToken.None);
            };
        }
        
        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Theory]
    [InlineData(QueueHandlerType.Azure_ServiceBus)]
    public async Task CountActiveMessagesInQueue_ShouldThrowException_IfConfigIsEmpty(QueueHandlerType type)
    {
        // Arrange
        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetQueueHandlerConfigurationByTypeQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult("{\"ConnectionString\":\"\",\"QueueName\":\"\"}"));

        IQueueHandler? handler = QueueHandlerFactory.GetHandler(type, mediator.Object, _logger.Object);

        // Act
        Func<Task>? act = null;
        if (handler != null)
        {
            act = async () =>
            {
                await handler!.CountActiveMessagesInQueue(CancellationToken.None);
            };
        }


        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Theory]
    [InlineData(QueueHandlerType.Azure_ServiceBus)]
    public async Task ReadMessagesFromQueue_ShouldThrowException__IfConfigIsEmpty(QueueHandlerType type)
    {
        // Arrange
        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetQueueHandlerConfigurationByTypeQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult("{\"ConnectionString\":\"\",\"QueueName\":\"\"}"));

        IQueueHandler? handler = QueueHandlerFactory.GetHandler(type, mediator.Object, _logger.Object);

        // Act
        Func<Task>? act = null;
        if (handler != null)
        {
            act = async () =>
            {
                await handler!.ReadMessagesFromQueue(false, CancellationToken.None);
            };
        }


        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Theory]
    [InlineData(QueueHandlerType.Azure_ServiceBus)]
    public async Task AddMessageToQueue_ShouldThrowException__IfConfigIsEmpty(QueueHandlerType type)
    {
        // Arrange
        var dummyJson = "{\"ConnectionString\":\"aaaa\",\"QueueName\":\"bbb\"}";

        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetQueueHandlerConfigurationByTypeQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult("{\"ConnectionString\":\"\",\"QueueName\":\"\"}"));

        IQueueHandler? handler = QueueHandlerFactory.GetHandler(type, mediator.Object, _logger.Object);

        // Act
        Func<Task>? act = null;
        if (handler != null)
        {
            act = async () =>
            {
                await handler!.AddMessageToQueue(dummyJson, CancellationToken.None);
            };
        }
        
        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

}
