using Hibit.Application.Contact;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Hibit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContactController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContactController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [EnableRateLimiting("contact")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> Submit([FromBody] ContactMessageDto dto, CancellationToken cancellationToken)
    {
        var command = new SubmitContactCommand(
            dto.Name,
            dto.Email,
            dto.Phone,
            dto.Subject,
            dto.Message,
            dto.ConsentGiven);

        await _mediator.Send(command, cancellationToken);
        return Accepted(new { message = "Mensagem recebida com sucesso." });
    }
}
