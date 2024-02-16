using Microsoft.AspNetCore.Mvc;
using NotificationHub.Application.QueueHandlers;
using NotificationHub.Application.Queues.Commands;
using NotificationHub.Application.Queues.Queries;

namespace NotificationHub.API.Controllers;

public class QueueController : BaseController
{
    private readonly MediatR.ISender _mediator;
    private readonly IQueueHandler? _queueHandler;

    public QueueController(MediatR.ISender mediator, ILogger<QueueController> logger)
        : base(logger)
    {
        _mediator = mediator
            ?? throw new ArgumentNullException(nameof(mediator));

        _queueHandler = QueueHandlerFactory.GetHandler(type: QueueHandlerType.Azure_ServiceBus,
            mediator: _mediator,
            logger: Logger);
    }

    [HttpPost("SendNotification/{jsonMessage}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    public async Task<IActionResult> SendMessage(string jsonMessage) 
    {
        var result = await _mediator.Send(new AddMessageToQueueCommand(_queueHandler, jsonMessage));
        return Ok(result);
    }

    [HttpGet("Messages")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dictionary<string, string>))]
    public async Task<IActionResult> GetPendinMessages() 
    {
        var messages= await _mediator.Send(new ReadMessagesFromQueueQuery(_queueHandler));
        return Ok(messages);
    }

    [HttpGet("MessagesCount")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> GetQueueActiveMessageCount() 
    {
        var count = await _mediator.Send(new GetActiveMessagesCountQuery(_queueHandler));
        return Ok(count);
    }
}
