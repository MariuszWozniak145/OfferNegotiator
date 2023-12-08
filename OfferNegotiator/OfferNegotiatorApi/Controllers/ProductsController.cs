using MediatR;
using Microsoft.AspNetCore.Mvc;
using OfferNegotiatorLogic.CQRS.Product.Queries;
using OfferNegotiatorLogic.DTOs.Product;

namespace OfferNegotiatorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Endpoint Description
    /// <summary>
    /// Retrieves the products.
    /// </summary>
    /// <returns>
    ///   Returns an HTTP 200 (OK) response with the products.
    /// </returns>
    /// <remarks>
    ///   This endpoint allows you to retrieve products.
    ///   After a successful retrieval, a response with an HTTP 200 (OK) status code will be
    ///   returned, and it will contain the products.
    /// </remarks>
    /// <response code="200">The products was successfully retrieved.</response>
    [ProducesResponseType(typeof(List<ProductReadDTO>), StatusCodes.Status200OK)]
    #endregion
    [HttpGet()]
    public async Task<IActionResult> GetAllProducts()
    {
        var result = await _mediator.Send(new GetProductsQuery());
        return Ok(result);
    }
}