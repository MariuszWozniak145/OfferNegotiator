using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfferNegotiatorDal.Models.Enums;
using OfferNegotiatorLogic.CQRS.Product.Commands;
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
    ///     Retrieves the products.
    /// </summary>
    /// <returns>
    ///     Returns an HTTP 200 (OK) response with the products.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows you to retrieve products.
    ///     After a successful retrieval, a response with an HTTP 200 (OK) status code will be
    ///     returned, and it will contain the products.
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
    ///     Retrieves a product with its offers.
    /// </summary>
    /// <param name="id">The unique identifier of the product to be retrieved.</param>
    /// <returns>
    ///     Returns an HTTP 200 (OK) response with the product and its offers.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows you to retrieve product with its offers by providing the unique identifier ("id")
    ///     of the product as a part of the URL route.
    ///     After a successful retrieval, a response with an HTTP 200 (OK) status code will be
    ///     returned, and it will contain the product with its offers.
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

    #region Endpoint Description
    /// <summary>
    ///     Retrieves the available products.
    /// </summary>
    /// <returns>
    ///     Returns an HTTP 200 (OK) response with the available products.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows you to retrieve available products.
    ///     After a successful retrieval, a response with an HTTP 200 (OK) status code will be
    ///     returned, and it will contain the available products.
    /// </remarks>
    /// <response code="200">The available products was successfully retrieved.</response>
    [ProducesResponseType(typeof(ProductWithOffersReadDTO), StatusCodes.Status200OK)]
    #endregion
    [HttpGet("Available")]
    public async Task<IActionResult> GetAvailableProducts()
    {
        var result = await _mediator.Send(new GetProductWithSpecifiedStateQuery(ProductState.Available));
        return Ok(result);
    }

    #region Endpoint Description
    /// <summary>
    ///     Retrieves the sold products.
    /// </summary>
    /// <returns>
    ///     Returns an HTTP 200 (OK) response with the sold products.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows you to retrieve sold products.
    ///     After a successful retrieval, a response with an HTTP 200 (OK) status code will be
    ///     returned, and it will contain the sold products.
    /// </remarks>
    /// <response code="200">The sold products was successfully retrieved.</response>
    [ProducesResponseType(typeof(ProductWithOffersReadDTO), StatusCodes.Status200OK)]
    #endregion
    [HttpGet("Sold")]
    public async Task<IActionResult> GetSoldProducts()
    {
        var result = await _mediator.Send(new GetProductWithSpecifiedStateQuery(ProductState.Sold));
        return Ok(result);
    }

    #region Endpoint Description
    /// <summary>
    ///     Deletes an existing product by its unique identifier (id).
    /// </summary>
    /// <param name="id">The unique identifier of the product to be deleted.</param>
    /// <returns>
    ///     Returns an HTTP 204 (No Content) response upon successful deletion of the product.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows you to delete an existing product by providing the unique identifier ("id") of the product
    ///     as part of the URL route. To use this endpoint, ensure that you are authenticated with a valid authorization token
    ///     and you have enough permissions (only the employee can remove the product),
    ///     as it is secured with the "Authorize" attribute. After successful deletion, a response with an HTTP 204 (No Content)
    ///     status code will be returned.
    /// </remarks>
    /// <response code="204">The product with the specified "id" was successfully deleted, and no content is returned.</response>
    /// <response code="401">User was unauthorized or JWT was invalid.</response>
    /// <response code="401">User does not have enough permissions (only the employee can remove the product).</response>
    /// <response code="404">The product with the specified "id" was not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status404NotFound)]
    #endregion
    [HttpDelete("{id}")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }
}