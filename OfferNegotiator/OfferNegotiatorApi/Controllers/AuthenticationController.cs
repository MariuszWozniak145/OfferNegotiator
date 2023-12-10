using MediatR;
using Microsoft.AspNetCore.Mvc;
using OfferNegotiatorLogic.CQRS.Authentication.Commands.Post;
using OfferNegotiatorLogic.DTOs.Exception;
using OfferNegotiatorLogic.DTOs.Login;

namespace OfferNegotiatorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Endpoint Description
    /// <summary>
    ///     Logs in a user to their account.
    /// </summary>
    /// <param name="loginCreateDTO">Data for user login.</param>
    /// <returns>
    ///      Returns an HTTP 200 (OK) response upon successful user login along with user authentication data.
    /// </returns>
    /// <remarks>
    ///   This endpoint allows a registered user to log in to their account by providing their login credentials
    ///   in the request body using the JSON format. After successful login a response with an HTTP 200 (OK)
    ///   status code will be returned with user authentication data such as tokens or user information.
    /// </remarks>
    /// <response code="200">The user was successfully logged in, and user authentication data is returned with JWT and refresh token.</response>
    /// <response code="400">The login request was invalid or the login data is incorrect.</response>
    /// <response code="500">The error occurred on the server side.</response>
    [ProducesResponseType(typeof(LoginReadDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status500InternalServerError)]
    #endregion
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginCreateDTO loginCreateDTO)
    {
        var result = await _mediator.Send(new LoginCommand(loginCreateDTO));
        return Ok(result);
    }
}