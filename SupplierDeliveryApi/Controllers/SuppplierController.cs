using AuthAPI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupplierDeliveryAPI.Application.Contracts;
using SupplierDeliveryAPI.Domain;
using SupplierDeliveryAPI.Models;
using Utils;
using Utils.DomainNotification;

namespace SupplierDeliveryApi.Controllers
{
    /// <summary>
    /// Controller for managing suppliers.
    /// </summary>
    public class SupplierController : BaseController
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(INotifier notifier, ISupplierService supplierService, IHttpContextAccessor httpContext) : base(notifier, httpContext)
        {
            _supplierService = supplierService;
        }

        /// <summary>
        /// Get all suppliers.
        /// </summary>
        /// <returns></returns>
        [HttpGet("suppliers")]
        [ProducesResponseType(typeof(List<Supplier>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult GetAllSuppliers()
        {
            return ApiReturn(async () =>
            {
                return await _supplierService.GetAllSuppliers();
            }, StatusCodes.Status200OK);
        }

        /// <summary>
        /// GET a supplier by its ID.
        /// </summary>
        /// <param name="idSupplier"></param>
        /// <returns></returns>
        [HttpGet("suppliers/{idSupplier}")]
        [ProducesResponseType(typeof(Supplier), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult GetSupplierById(int idSupplier)
        {
            return ApiReturn(async () =>
            {
                return await _supplierService.GetSupplierById(idSupplier);
            }, StatusCodes.Status200OK);
        }

        /// <summary>
        /// Post a new supplier.
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        [HttpPost("suppliers")]
        [ProducesResponseType(typeof(ResponseClass), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult AddSupplier([FromBody] SupplierDTO supplier)
        {
            return ApiReturn(async () =>
            {
                return await _supplierService.AddAsync(supplier);
            }, StatusCodes.Status201Created);
        }
    }
}
