using FluentValidation;
using MediatR;
using NotificationHub.Application.Notification.Models;

namespace NotificationHub.Application.Notification.Commands;

public record SendNotificationCommand(Message? Message, Senders.ISender? Sender)
    : IRequest<Unit>;

public class SendNotificationCommandValidator
    : AbstractValidator<SendNotificationCommand>
{
    public SendNotificationCommandValidator()
    {
        RuleFor(r => r.Sender)
            .NotNull()
            .WithMessage("Sender can not be null");

        RuleFor(r => r.Message)
            .Custom((m, context) =>
            {
                if (m == null)
                    context.AddFailure("Message", "Message can not be null");
                else
                {
                    if (string.IsNullOrWhiteSpace(m.Body))
                        context.AddFailure("Body", "Message body can not be null or empty");

                    if (string.IsNullOrWhiteSpace(m.To))
                        context.AddFailure("To", "Message to can not be null or empty");
                }
            });
    }
}

public class SendNotificationCommandHandler
    : IRequestHandler<SendNotificationCommand, Unit>
{
    public async Task<Unit> Handle(SendNotificationCommand command, CancellationToken cancellationToken)
    {
        await command.Sender!
            .SendAsync(command.Message.To, command.Message.Subject, command.Message.Body, cancellationToken);

        return Unit.Value;
    }
}

