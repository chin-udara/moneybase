using Microsoft.AspNetCore.Mvc;
using MoneybaseChat.application.iServices;

namespace MoneybaseChat.api;

[ApiController]
[Route("api/[controller]")]
public class MoneyBaseController(IInitiateChatService initiateChatService, IChatRequestPulseService chatRequestPulseService) : ControllerBase
{
    private readonly IInitiateChatService initiateChatService = initiateChatService;
    private readonly IChatRequestPulseService chatRequestPulseService = chatRequestPulseService;

    [HttpGet]
    public async Task<IActionResult> ChatRequest(CancellationToken cancellationToken)
    {
        var requestId = await initiateChatService.Initiate(cancellationToken);
        if (requestId is null)
            return StatusCode(503, "All agents are busy right now. Please try again later");
        return Ok(requestId);
    }

    [HttpPatch("{chatIdentifier}/pulsate")]
    public async Task<IActionResult> Pulsate(string chatIdentifier, CancellationToken cancellationToken)
    {
        var parsedIdentifier = Guid.Parse(chatIdentifier);
        await chatRequestPulseService.Pulsate(parsedIdentifier, cancellationToken);
        return Ok();
    }
}
