using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationHub.API.Controllers;
using NotificationHub.API.Test.Mocks;
using NotificationHub.Application.Queues.Queries;

namespace NotificationHub.API.Test.Controllers;

public class QueueControllerTests
{
    private const string host = "http://localhost:5188";
    private const string pathBase = "/api";

    public QueueControllerTests() { }

    [Fact]
    public async Task SendMessage_ShouldReturnOk_IfAllParamsAreCorrect()
    {
        // Arrange
        var jsonMessage = "{\"a\":\"a\",\"v\":\"v\"}";

        var request = MockHttpRequest.GetMock(host, pathBase);

        var context = Mock.Of<HttpContext>(_ =>
            _.Request == request.Object
        );

        var controllerContext = new ControllerContext()
        {
            HttpContext = context
        };

        var logger = new Mock<ILogger<QueueController>>();

        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetActiveMessagesCountQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        var controller = new QueueController(mediator: mediator.Object,
            logger: logger.Object)
        {
            ControllerContext = controllerContext
        };

        // Act
        var result = await controller.SendMessage(jsonMessage);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetPendinMessages_ShouldReturnOk_IfAllParamsAreCorrect()
    {
        // Arrange
        var request = MockHttpRequest.GetMock(host, pathBase);

        var context = Mock.Of<HttpContext>(_ =>
            _.Request == request.Object
        );

        var controllerContext = new ControllerContext()
        {
            HttpContext = context
        };

        var logger = new Mock<ILogger<QueueController>>();

        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetActiveMessagesCountQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        var controller = new QueueController(mediator: mediator.Object,
            logger: logger.Object)
        {
            ControllerContext = controllerContext
        };

        // Act
        var result = await controller.GetPendinMessages();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetQueueActiveMessageCount_ShouldReturnOk_IfAllParamsAreCorrect()
    {
        // Arrange
        var request = MockHttpRequest.GetMock(host, pathBase);

        var context = Mock.Of<HttpContext>(_ =>
            _.Request == request.Object
        );

        var controllerContext = new ControllerContext()
        {
            HttpContext = context
        };

        var logger = new Mock<ILogger<QueueController>>();

        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetActiveMessagesCountQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        var controller = new QueueController(mediator: mediator.Object,
            logger: logger.Object)
        {
            ControllerContext = controllerContext
        };

        // Act
        var result = await controller.GetQueueActiveMessageCount();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
}
