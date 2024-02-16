using FluentValidation;
using MediatR;
using NotificationHub.Application.QueueHandlers;

namespace NotificationHub.Application.Queues.Queries;

public record ReadMessagesFromQueueQuery(IQueueHandler? QueueHandler, bool markAsProccessed = true)
    : IRequest<Dictionary<string, string>>;

public class ReadMessagesFromQueueQueryValidator
    : AbstractValidator<ReadMessagesFromQueueQuery>
{
    public ReadMessagesFromQueueQueryValidator()
    {
        RuleFor(r => r.QueueHandler)
            .NotNull()
            .WithMessage("QueueHandler can not be Null");
    }
}

public class ReadMessagesFromQueueQueryHandler
    : IRequestHandler<ReadMessagesFromQueueQuery, Dictionary<string, string>>
{
    public async Task<Dictionary<string, string>> Handle(ReadMessagesFromQueueQuery query, CancellationToken cancellationToken)
    {
        return await query.QueueHandler!.ReadMessagesFromQueue(query.markAsProccessed, cancellationToken);
    }
}