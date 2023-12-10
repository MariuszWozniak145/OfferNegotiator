using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfferNegotiatorDal.Models.Enums;
using OfferNegotiatorLogic.CQRS.Product.Commands.Delete;
using OfferNegotiatorLogic.CQRS.Product.Commands.Post;
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
    ///     This endpoint allows to retrieve products.
    ///     After a successful retrieval a response with an HTTP 200 (OK) status code will be
    ///     returned and it will contain the products.
    /// </remarks>
    /// <response code="200">The products were successfully retrieved.</response>
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
    /// <param name="productId">The unique identifier of the product to be retrieved.</param>
    /// <returns>
    ///     Returns an HTTP 200 (OK) response with the product and its offers.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows to retrieve product with its offers by providing the unique identifier ("productId")
    ///     of the product as a part of the URL route.
    ///     After a successful retrieval a response with an HTTP 200 (OK) status code will be
    ///     returned and it will contain the product with its offers.
    /// </remarks>
    /// <response code="200">The product with the specified "productId" and its offers were successfully retrieved.</response>
    /// <response code="404">The product with the specified "productId" was not found.</response>
    [ProducesResponseType(typeof(ProductWithOffersReadDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status404NotFound)]
    #endregion
    [HttpGet("{productId}/WithOffers")]
    public async Task<IActionResult> GetProductWithOffers(Guid productId)
    {
        var result = await _mediator.Send(new GetProductWithOffersQuery(productId));
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
    ///     This endpoint allows to retrieve available products.
    ///     After a successful retrieval a response with an HTTP 200 (OK) status code will be
    ///     returned and it will contain the available products.
    /// </remarks>
    /// <response code="200">The available products were successfully retrieved.</response>
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
    ///     This endpoint allows to retrieve sold products.
    ///     After a successful retrieval a response with an HTTP 200 (OK) status code will be
    ///     returned and it will contain the sold products.
    /// </remarks>
    /// <response code="200">The sold products were successfully retrieved.</response>
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
    ///     Deletes an existing product by its unique identifier (productId).
    /// </summary>
    /// <param name="productId">The unique identifier of the product to be deleted.</param>
    /// <returns>
    ///     Returns an HTTP 204 (No Content) response upon successful deletion of the product.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows to delete an existing product by providing the unique identifier ("productId") of the product
    ///     as part of the URL route. To use this endpoint ensure that you are authenticated with a valid authorization token
    ///     and you have enough permissions (only the employee can delete the product)
    ///     as it is secured with the "Authorize" attribute. After successful deletion a response with an HTTP 204 (No Content)
    ///     status code will be returned.
    /// </remarks>
    /// <response code="204">The product with the specified "productId" was successfully deleted and no content is returned.</response>
    /// <response code="401">User was unauthorized or JWT was invalid.</response>
    /// <response code="403">User does not have enough permissions (only the employee can delete the product).</response>
    /// <response code="404">The product with the specified "productId" was not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status404NotFound)]
    #endregion
    [HttpDelete("{productId}")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        await _mediator.Send(new DeleteProductCommand(productId));
        return NoContent();
    }

    #region Endpoint Description
    /// <summary>
    ///     Creates a new product.
    /// </summary>
    /// <param name="product">Data for creating a new product.</param>
    /// <returns>
    ///     Returns an HTTP 201 (Created) response upon successful creation of a new product along with the product details.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows to create a new product by providing the necessary product data in the request body using
    ///     the JSON format. To use this endpoint ensure that you are authenticated with a valid authorization token
    ///     and you have enough permissions (only the employee can create the product)  
    ///     as it is secured with the "Authorize" attribute. After successful creation a response with an HTTP 201 (Created) status code
    ///     will be returned and it will include the details of the newly created product.
    /// </remarks>
    /// <response code="201">The new product was successfully created and its details are returned.</response>
    /// <response code="400">The creation request was invalid or the product data is incomplete.</response>
    /// <response code="401">User was unauthorized or JWT was invalid.</response>
    /// <response code="403">User does not have enough permissions (only the employee can create the product).</response>
    /// <response code="500">The error occurred on the server side.</response>
    [ProducesResponseType(typeof(ProductWithOffersReadDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status500InternalServerError)]
    #endregion
    [HttpPost]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDTO product)
    {
        var result = await _mediator.Send(new CreateProductCommand(product));
        return CreatedAtAction(nameof(GetProductWithOffers), new { productId = result.Id }, result);
    }
}