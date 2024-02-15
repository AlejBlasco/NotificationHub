using Microsoft.AspNetCore.Mvc;

namespace NotificationHub.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : Controller
{
    private readonly ILogger logger;

    protected ILogger Logger => logger;

    public BaseController(ILogger logger)
    {
        this.logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }
}
