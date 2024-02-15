using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationHub.Application.Notification.Commands;

namespace NotificationHub.Application.Test.Notification;

public class INotificationHubTests
{
    [Theory]
    [InlineData(NotificationType.Email_SMTP)]
    [InlineData(NotificationType.Email_O365)]
    [InlineData(NotificationType.SMS)]
    [InlineData(NotificationType.WhatsApp)]
    public void Send_ShouldReturnOk_IfAllParamsAreCorrect(NotificationType notificationType)
    {
        // Arrange
        var logger = new Mock<ILogger<NotificationHub>>();

        var mediator = new Mock<ISender>();
        mediator.Setup(x => x.Send(It.IsAny<SendNotificationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        INotificationHub notificationHub = new NotificationHub(mediator.Object, logger.Object);
        var result = notificationHub.SendNotification("messageBody", "messageSubject", "messageTo", notificationType);

        // Assert
        result.IsCompleted.Should().BeTrue();
        result.IsCompletedSuccessfully.Should().BeTrue();
    }

    [Theory]
    [InlineData(NotificationType.Email_SMTP)]
    [InlineData(NotificationType.Email_O365)]
    [InlineData(NotificationType.SMS)]
    [InlineData(NotificationType.WhatsApp)]
    public void Send_ShouldReturnOk_IfOnlyMandatoryParams(NotificationType notificationType)
    {
        // Arrange
        var logger = new Mock<ILogger<NotificationHub>>();

        var mediator = new Mock<ISender>();
        mediator.Setup(x => x.Send(It.IsAny<SendNotificationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        INotificationHub notificationHub = new NotificationHub(mediator.Object, logger.Object);
        var result = notificationHub.SendNotification("messageBody", "", "messageTo", notificationType);

        // Assert
        result.IsCompleted.Should().BeTrue();
        result.IsCompletedSuccessfully.Should().BeTrue();
    }
}
