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
    /// Controller for managing deliveries.
    /// </summary>
    public class DeliveryController : BaseController
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(INotifier notifier, IDeliveryService deliveryService, IHttpContextAccessor httpContext) : base(notifier, httpContext)
        {
            _deliveryService = deliveryService;
        }

        /// <summary>
        ///  Get deliveries for a specific supplier within an optional date range.
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("deliveries")]
        [ProducesResponseType(typeof(List<DeliveryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult GetDeliveries(int supplierId, DateTime? startDate = null, DateTime? endDate = null)
        {
            return ApiReturn(async () =>
            {
                return await _deliveryService.GetDeliveries(supplierId, startDate, endDate);
            }, StatusCodes.Status200OK);
        }

        /// <summary>
        /// Get deliveries by supplier ID.
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        [HttpGet("deliveries/supplier/{supplierId}")]
        [ProducesResponseType(typeof(List<DeliveryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult GetDeliveriesBySupplier(int supplierId)
        {
            return ApiReturn(async () =>
            {
                return await _deliveryService.GetDeliveriesBySupplier(supplierId);
            }, StatusCodes.Status200OK);
        }


        /// <summary>
        /// Get a delivery by its ID.
        /// </summary>
        /// <param name="idDelivery"></param>
        /// <returns></returns>
        [HttpGet("deliveries/{idDelivery}")]
        [ProducesResponseType(typeof(DeliveryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult GetDeliveryById(int idDelivery)
        {
            return ApiReturn(async () =>
            {
                return await _deliveryService.GetDeliveryById(idDelivery);
            }, StatusCodes.Status200OK);
        }

        /// <summary>
        /// Post a new delivery.
        /// </summary>
        /// <param name="delivery"></param>
        /// <returns></returns>
        [HttpPost("deliveries")]
        [ProducesResponseType(typeof(DeliveryResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult AddDelivery([FromBody] DeliveryRequest delivery)
        {
            return ApiReturn(async () =>
            {
                return await _deliveryService.AddDelivery(delivery);
            }, StatusCodes.Status201Created);
        }

        /// <summary>
        /// Update an existing delivery.
        /// </summary>
        /// <param name="delivery"></param>
        /// <returns></returns>
        [HttpPut("deliveries")]
        [ProducesResponseType(typeof(DeliveryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public IActionResult UpdateDelivery([FromBody] DeliveryRequest delivery)
        {
            return ApiReturn(async () =>
            {
                return await _deliveryService.UpdateDelivery(delivery);
            }, StatusCodes.Status200OK);
        }
    }
}
