using FluentValidation;
using MediatR;
using NotificationHub.Application.QueueHandlers;

namespace NotificationHub.Application.Queues.Queries;

public record GetActiveMessagesCountQuery(IQueueHandler? QueueHandler)
    : IRequest<int>;

public class GetActiveMessagesCountQueryValidator
    : AbstractValidator<GetActiveMessagesCountQuery>
{
    public GetActiveMessagesCountQueryValidator()
    {
        RuleFor(r => r.QueueHandler)
            .NotNull()
            .WithMessage("QueueHandler can not be Null");
    }
}

public class GetActiveMessagesCountQueryHandler
    : IRequestHandler<GetActiveMessagesCountQuery, int>
{
    public async Task<int> Handle(GetActiveMessagesCountQuery query, CancellationToken cancellationToken)
    {
        return await query.QueueHandler!.CountActiveMessagesInQueue(cancellationToken);
    }
}
