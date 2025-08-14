using AuthAPI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupplierDeliveryAPI.Application.Contracts;
using SupplierDeliveryAPI.Models;
using Utils;
using Utils.DomainNotification;

namespace SupplierDeliveryAPI.Controllers
{
    /// <summary>
    /// Controller for managing products.
    /// </summary>
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(INotifier notifier, IProductService productService, IHttpContextAccessor httpContext) : base(notifier, httpContext)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        /// <returns></returns>
        [HttpGet("products")]
        [ProducesResponseType(typeof(List<ProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult GetProducts()
        {
            return ApiReturn(async () =>
            {
                List<ProductDTO> products = await _productService.GetProducts();
                return products;
            }, StatusCodes.Status200OK);
        }
        /// <summary>
        /// Get a product by its ID.
        /// </summary>
        /// <param name="idProduct"></param>
        /// <returns></returns>

        [HttpGet("products/{idProduct}")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult GetProductById(int idProduct)
        {
            return ApiReturn(async () =>
            {
                ProductDTO product = await _productService.GetProductById(idProduct);
                return product;
            }, StatusCodes.Status200OK);
        }

        /// <summary>
        /// Post a new product.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("products")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult AddProduct([FromBody] ProductDTO product)
        {
            return ApiReturn(async () =>
            {
                ProductDTO addedProduct = await _productService.AddProduct(product);
                return addedProduct;
            }, StatusCodes.Status201Created);
        }


        /// <summary>
        /// Update an existing product.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("products")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult UpdateProduct([FromBody] ProductDTO product)
        {
            return ApiReturn(async () =>
            {
                ProductDTO updatedProduct = await _productService.UpdateProduct(product);
                return updatedProduct;
            }, StatusCodes.Status200OK);
        }

        /// <summary>
        /// Delete a product by its ID.
        /// </summary>
        /// <param name="idProduct"></param>
        /// <returns></returns>
        [HttpDelete("products/{idProduct}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult DeleteProduct(int idProduct)
        {
            return ApiReturn(async () =>
            {
                bool deleted = await _productService.DeleteProduct(idProduct);
                return deleted;
            }, StatusCodes.Status200OK);
        }
    }
}