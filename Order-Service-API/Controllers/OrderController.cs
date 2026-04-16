using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order_Service_Application.UseCases.CreateOrder;
using Order_Service_Application.UseCases.GetOrders;

namespace Order_Service_API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
        {
            var orderId = await _mediator.Send(command);

            return Ok(new { orderId });
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _mediator.Send(new GetOrdersQuery());
            return Ok(response);
        }

    }

}
