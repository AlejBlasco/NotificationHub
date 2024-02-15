using FluentValidation;
using MediatR;
using NotificationHub.Application.Notification.Models;

namespace NotificationHub.Application.Notification.Commands;

internal record SendNotificationCommand(Message Message, Senders.ISender? Sender)
    : IRequest<Unit>;

internal class SendNotificationCommandValidator
    : AbstractValidator<SendNotificationCommand>
{
    internal SendNotificationCommandValidator()
    {
        RuleFor(r => r.Sender)
            .NotNull()
            .WithMessage("Sender can not be null");

        RuleFor(r => r.Message)
            .NotNull()
            .WithMessage("Message can not be null");

        RuleFor(r => r.Message.Body)
            .NotNull()
            .NotEmpty()
            .WithMessage("Message body can not be null or empty");

        RuleFor(r => r.Message.To)
            .NotNull()
            .NotEmpty()
            .WithMessage("Message To can not be null or empty");
    }
}

internal class SendNotificationCommandHandler
    : IRequestHandler<SendNotificationCommand, Unit>
{
    public async Task<Unit> Handle(SendNotificationCommand command, CancellationToken cancellationToken)
    {
        await command.Sender!
            .SendAsync(command.Message.To, command.Message.Subject, command.Message.Body, cancellationToken);

        return Unit.Value;
    }
}

