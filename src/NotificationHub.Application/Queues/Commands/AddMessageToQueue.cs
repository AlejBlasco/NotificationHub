using FluentValidation;
using MediatR;
using NotificationHub.Application.QueueHandlers;

namespace NotificationHub.Application.Queues.Commands;

public record AddMessageToQueueCommand(IQueueHandler? QueueHandler, string? jsonMessage)
    : IRequest<bool>;

public class AddMessageToQueueCommandValidator
    : AbstractValidator<AddMessageToQueueCommand>
{
    public AddMessageToQueueCommandValidator()
    {
        RuleFor(r => r.QueueHandler)
            .NotNull()
            .WithMessage("QueueHandler can not be Null");

        RuleFor(r => r.jsonMessage)
            .Custom((m, context) =>
            {
                if (string.IsNullOrWhiteSpace(m))
                    context.AddFailure("jsonMessage", "jsonMessage can not be null or empty");
                else
                {
                    //TODO: Add Json Validation method.
                }
            });
    }
}

public class AddMessageToQueueCommandHandler
    : IRequestHandler<AddMessageToQueueCommand, bool>
{
    public async Task<bool> Handle(AddMessageToQueueCommand command, CancellationToken cancellationToken)
    {
        return await command.QueueHandler!.AddMessageToQueue(command.jsonMessage!, cancellationToken);
    }
}
