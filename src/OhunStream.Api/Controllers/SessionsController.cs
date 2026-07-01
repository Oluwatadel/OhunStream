using Dispatcher;
using Microsoft.AspNetCore.Mvc;
using static OhunStream.Application.Commands.StartSessionCommand;

namespace OhunStream.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController(ISender sender) : ControllerBase
    {
        [HttpPost("start")]
        public async Task<IActionResult> Start(StartSessionRequest request, CancellationToken cancellationToken = default)
        {
            var result = await sender.Send<StartSessionRequest, StartSessionResponse>(request, cancellationToken);
            return Ok(result);
        }
    }
}