using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationHub.API.Controllers;
using NotificationHub.API.Test.Mocks;
using NotificationHub.Application;

namespace NotificationHub.API.Test.Controllers;

public class NotificationControllerTests
{
    private const string host = "http://localhost:5188";
    private const string pathBase = "/api";

    public NotificationControllerTests() { }

    [Theory]
    [InlineData(NotificationType.Email_SMTP)]
    [InlineData(NotificationType.Email_O365)]
    [InlineData(NotificationType.SMS)]
    [InlineData(NotificationType.WhatsApp)]
    public async Task SendNotification_ShouldReturnOk_IfAllParamsAreCorrect(NotificationType notificationType)
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

        var logger = new Mock<ILogger<NotificationController>>();
        var notificationHub = new Mock<INotificationHub>();

        var controller = new NotificationController(logger: logger.Object,
            notificationHub: notificationHub.Object)
        {
            ControllerContext = controllerContext
        };

        // Act
        var result = await controller.SendNotification(senderType: notificationType,
             messageTo: "messageTo",
             messageBody: "messageBody",
             messageSubject: "messageSubject");

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Theory]
    [InlineData(NotificationType.Email_SMTP)]
    [InlineData(NotificationType.Email_O365)]
    [InlineData(NotificationType.SMS)]
    [InlineData(NotificationType.WhatsApp)]
    public async Task SendNotification_ShouldReturnOk_IfOnlyMandatoryParams(NotificationType notificationType)
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

        var logger = new Mock<ILogger<NotificationController>>();
        var notificationHub = new Mock<INotificationHub>();

        var controller = new NotificationController(logger: logger.Object,
            notificationHub: notificationHub.Object)
        {
            ControllerContext = controllerContext
        };

        // Act
        var result = await controller.SendNotification(senderType: notificationType,
             messageTo: "messageTo",
             messageBody: "messageBody");

        // Assert
        result.Should().BeOfType<OkResult>();
    }

}
