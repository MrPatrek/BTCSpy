using BTCSpy.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;

namespace BTCSpy.Presentation.Controllers
{
    [Route("api/btcspy")]
    [ApiController]
    public class BTCSpyController : ControllerBase
    {
        private readonly IServiceManager _service;

        public BTCSpyController(IServiceManager service) => _service = service;

        [HttpGet]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult GetBestPriceOrders([FromQuery] BestPriceOrdersQueryParametersDto bestPriceOrdersQueryParameters)
        {
            if (!(
                bestPriceOrdersQueryParameters.Type.Equals(Constants.BuyStr) ||
                bestPriceOrdersQueryParameters.Type.Equals(Constants.SellStr)
                ))
                return BadRequest("Type parameter must be either Buy or Sell.");

            var bestPriceOrders = _service.BTCSpyService.GetBestPriceOrders(bestPriceOrdersQueryParameters);

            return Ok(bestPriceOrders);
        }
    }
}
