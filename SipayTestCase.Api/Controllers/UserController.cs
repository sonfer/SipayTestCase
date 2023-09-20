using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SipayTestCase.Application.Cqrs.Commands.User;
using SipayTestCase.Application.Cqrs.Queries.User;
using SipayTestCase.Contract.Dtos;
using SipayTestCase.Shared.Interfaces;
using IResult = SipayTestCase.Shared.Interfaces.IResult;

namespace SipayTestCase.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMediator _mediator;

    public UserController(ILogger<UserController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(IDataResult<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] UserRegisterCommand request)
    {
        return Ok(await _mediator.Send(request));
    }
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(IDataResult<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] UserLoginQuery request)
    {
        return Ok(await _mediator.Send(request));
    }
    
    [HttpPost("activation")]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Activation([FromBody] UserEmailActivationCommand request)
    {
        return Ok(await _mediator.Send(request));
    }
    
    [HttpPost("update")]
    [Authorize]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody] UserUpdateCommand request)
    {
        return Ok(await _mediator.Send(request));
    }
}