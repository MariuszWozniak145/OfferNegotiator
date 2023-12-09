using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfferNegotiatorLogic.CQRS.Offers.Commands.Delete;
using OfferNegotiatorLogic.CQRS.Offers.Commands.Post;
using OfferNegotiatorLogic.CQRS.Offers.Queries;
using OfferNegotiatorLogic.DTOs.Exception;
using OfferNegotiatorLogic.DTOs.Offer;

namespace OfferNegotiatorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OffersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OffersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Endpoint Description
    /// <summary>
    ///     Retrieves a offer with a releted product.
    /// </summary>
    /// <param name="offerId">The unique identifier of the offer to be retrieved.</param>
    /// <returns>
    ///     Returns an HTTP 200 (OK) response with the offer with releted product.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows you to retrieve offer with releted product by providing the unique identifier ("offerId")
    ///     of the product as a part of the URL route.
    ///     After a successful retrieval, a response with an HTTP 200 (OK) status code will be
    ///     returned, and it will contain the offer with releted product.
    /// </remarks>
    /// <response code="200">The offer with the specified "offerId" was successfully retrieved.</response>
    /// <response code="404">The offer with the specified "offerId" was not found.</response>
    [ProducesResponseType(typeof(OfferWithProductReadDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status404NotFound)]
    #endregion
    [HttpGet("/{offerId}/WithProduct")]
    public async Task<IActionResult> GetOfferWithProduct(Guid offerId)
    {
        var result = await _mediator.Send(new GetOfferWithProductQuery(offerId));
        return Ok(result);
    }

    #region Endpoint Description
    /// <summary>
    ///     Retrieves the offers for specified client and product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="clientId">The unique identifier of the client.</param>
    /// <returns>
    ///     Returns an HTTP 200 (OK) response with the offers for specified client and product.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows you to retrieve offers for specified client and product by providing the unique 
    ///     identifiers ("productId" and "clientid") as a part of the URL route.
    ///     After a successful retrieval, a response with an HTTP 200 (OK) status code will be
    ///     returned, and it will contain the offers for specified client and product.
    /// </remarks>
    /// <response code="200">The offers for specified client ad product were successfully retrieved.</response>
    /// <response code="404">Product or client with the specified "id" was not found.</response>
    [ProducesResponseType(typeof(List<OfferReadDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status404NotFound)]
    #endregion
    [HttpGet("Product/{productId}/Client/{clientId}")]
    public async Task<IActionResult> GetClientOffersForProduct(Guid productId, Guid clientId)
    {
        var result = await _mediator.Send(new GetClientOffersForProductQuery(productId, clientId));
        return Ok(result);
    }

    #region Endpoint Description
    /// <summary>
    ///     Retrieves the offers for specified client.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client for whom the offers are to be retrieved.</param>
    /// <returns>
    ///     Returns an HTTP 200 (OK) response with the offers for specified client.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows you to retrieve offers for specified client by providing the unique 
    ///     identifier ("clientid") of the client as a part of the URL route.
    ///     After a successful retrieval, a response with an HTTP 200 (OK) status code will be
    ///     returned, and it will contain the offers for specified client.
    /// </remarks>
    /// <response code="200">The offers for specified client were successfully retrieved.</response>
    /// <response code="404">The client with the specified "clientId" was not found.</response>
    [ProducesResponseType(typeof(List<OfferWithProductReadDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status404NotFound)]
    #endregion
    [HttpGet("Client/{clientId}")]
    public async Task<IActionResult> GetClientOffers(Guid clientId)
    {
        var result = await _mediator.Send(new GetClientOffersQuery(clientId));
        return Ok(result);
    }

    #region Endpoint Description
    /// <summary>
    ///     Deletes an existing offer by its unique identifier (offerId).
    /// </summary>
    /// <param name="offerId">The unique identifier of the offer to be deleted.</param>
    /// <returns>
    ///     Returns an HTTP 204 (No Content) response upon successful deletion of the offer.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows you to delete an existing offer by providing the unique identifier ("offerId") of the offer
    ///     as part of the URL route. To use this endpoint, ensure that you are authenticated with a valid authorization token
    ///     and you have enough permissions (only the employee can delete the offer),
    ///     as it is secured with the "Authorize" attribute. After successful deletion, a response with an HTTP 204 (No Content)
    ///     status code will be returned.
    /// </remarks>
    /// <response code="204">The offer with the specified "offerId" was successfully deleted and no content is returned.</response>
    /// <response code="401">User was unauthorized or JWT was invalid.</response>
    /// <response code="403">User does not have enough permissions (only the employee can delete the product).</response>
    /// <response code="404">The offer with the specified "offerId" was not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status404NotFound)]
    #endregion
    [HttpDelete("{offerId}")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> DeleteOffer(Guid offerId)
    {
        await _mediator.Send(new DeleteOfferCommand(offerId));
        return NoContent();
    }

    #region Endpoint Description
    /// <summary>
    ///     Creates a new offer.
    /// </summary>
    /// <param name="offer">Data for creating a new offer.</param>
    /// <returns>
    ///     Returns an HTTP 201 (Created) response upon successful creation of a new offer, along with the offer details.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows you to create a new offer by providing the necessary offer data in the request body using
    ///     the JSON format. To use this endpoint, ensure that you are authenticated with a valid authorization token
    ///     and you have enough permissions (only the client can create the offer),  
    ///     as it is secured with the "Authorize" attribute. After successful creation, a response with an HTTP 201 (Created) status code
    ///     will be returned, and it will include the details of the newly created offer.
    /// </remarks>
    /// <response code="201">The new offer was successfully created, and its details are returned.</response>
    /// <response code="400">The creation request was invalid or the offer data is incorrect or client reached offers limit.</response>
    /// <response code="401">User was unauthorized or JWT was invalid.</response>
    /// <response code="403">User does not have enough permissions (only the client can create the product).</response>
    /// <response code="500">The error occurred on the server side.</response>
    [ProducesResponseType(typeof(OfferWithProductReadDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status500InternalServerError)]
    #endregion
    [HttpPost]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> CreateOffer([FromBody] OfferCreateDTO offer)
    {
        var result = await _mediator.Send(new CreateOfferCommand(offer, HttpContext.User.Claims));
        return CreatedAtAction(nameof(GetOfferWithProduct), new { offerId = result.Id }, result);
    }
}
