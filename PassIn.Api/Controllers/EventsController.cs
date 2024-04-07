using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PassIn.Api.UseCases.Events.GetById;
using PassIn.Api.UseCases.Events.Register;
using PassIn.Api.UseCases.Events.RegisterAttendee;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using System.Linq.Expressions;

namespace PassIn.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public IActionResult Register([FromBody] RequestEventJson request)
    {
        var UseCase = new RegisterEventUseCase();

        var response = UseCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public IActionResult GetById([FromRoute] Guid id)
    {
        var useCase = new GetEventByIdUseCase();

        var response = useCase.Execute(id);

        return Ok(response);
    }

    [HttpPost]
    [Route("{idEvent}/register")]
    [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
    public IActionResult Register([FromRoute] Guid idEvent, [FromBody] RequestRegisterEventJson request)
    {
        var useCase = new RegisterAttendeeOnEventUseCase();

        var response = useCase.Execute(idEvent, request);

        return Created(string.Empty, response);
    }
}
