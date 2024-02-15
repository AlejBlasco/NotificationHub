using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationHub.Application.Configuration.Queries;
using NotificationHub.Application.Senders;

namespace NotificationHub.Application.Test.Sender;

public class SenderFactoryTests
{
    private readonly Mock<ILogger> _logger;

    public SenderFactoryTests()
    {
        _logger = new Mock<ILogger>(MockBehavior.Loose);
    }

    [Theory]
    [InlineData(NotificationType.Email_SMTP)]
    [InlineData(NotificationType.Email_O365)]
    [InlineData(NotificationType.SMS)]
    //[InlineData(NotificationType.WhatsApp)]
    public void Factory_ShouldReturnISender_IfParamsAreCorrectAndSenderAreConfig(NotificationType notificationType)
    {
        // Arrange
        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetSenderConfigurationByTypeQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult("{\"EmailFrom\":\"\",\"User\":\"\",\"Password\":\"\",\"Server\":\"\",\"ServerPort\":0,\"EnableSSL\":false,\"EnableHtmlBody\":false}"));

        // Act
        SenderFactory.ClearCache();
        var result = SenderFactory.GetSender(notificationType, mediator.Object, _logger.Object);

        // Assert
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(NotificationType.Email_SMTP)]
    [InlineData(NotificationType.Email_O365)]
    [InlineData(NotificationType.SMS)]
    [InlineData(NotificationType.WhatsApp)]
    public void Factory_ShouldReturnNull_IfSenderAreNotConfig(NotificationType notificationType)
    {
        // Arrange
        var mediator = new Mock<MediatR.ISender>();
        mediator.Setup(x => x.Send(It.IsAny<GetSenderConfigurationByTypeQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(string.Empty));

        // Act
        SenderFactory.ClearCache();
        var result = SenderFactory.GetSender(notificationType, mediator.Object, _logger.Object);

        // Assert
        result.Should().BeNull();
    }
}
