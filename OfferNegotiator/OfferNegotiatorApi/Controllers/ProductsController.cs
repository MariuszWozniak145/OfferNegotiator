using MediatR;
using Microsoft.AspNetCore.Mvc;
using OfferNegotiatorLogic.CQRS.Product.Queries;
using OfferNegotiatorLogic.DTOs.Exception;
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

    #region Endpoint Description
    /// <summary>
    /// Retrieves a product with its offers.
    /// </summary>
    /// <param name="id">The unique identifier of the product to be retrieved.</param>
    /// <returns>
    ///   Returns an HTTP 200 (OK) response with the product and its offers.
    /// </returns>
    /// <remarks>
    ///   This endpoint allows you to retrieve product with its offers by providing the unique identifier ("id")
    ///   of the product as a part of the URL route.
    ///   After a successful retrieval, a response with an HTTP 200 (OK) status code will be
    ///   returned, and it will contain the product with its offers.
    /// </remarks>
    /// <response code="200">The product with the specified "id" and its offers was successfully retrieved.</response>
    /// <response code="404">The product with the specified "id" was not found.</response>
    [ProducesResponseType(typeof(ProductWithOffersReadDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status404NotFound)]
    #endregion
    [HttpGet("/{id}/WithOffers")]
    public async Task<IActionResult> GetProductWithOffers(Guid id)
    {
        var result = await _mediator.Send(new GetProductWithOffersQuery(id));
        return Ok(result);
    }
}