using Microsoft.AspNetCore.Mvc;
using SantaCruz.Domain.Service;
using SantaCruz.Domain.ViewModel.Order;
using SantaCruz.Domain.ViewModel.Page;

namespace SantaCruz.Order.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController(IOrderService orderService) : DetailedController
    {
        private readonly IOrderService _orderService = orderService;

        [HttpGet]
        public Task<IActionResult> Get([FromQuery] PageViewModel pageViewModel, CancellationToken cancellationToken) => 
            Handle(() => _orderService.List(pageViewModel, cancellationToken), Ok);

        [HttpGet("{id}")]
        public Task<IActionResult> Get(Guid id, CancellationToken cancellationToken) => 
            Handle(() => _orderService.Get(id, cancellationToken), Ok);

        [HttpPost]
        public Task<IActionResult> Post([FromBody] OrderCreateViewModel createViewModel, CancellationToken cancellationToken) =>
            Handle(() => _orderService.Create(createViewModel, cancellationToken), Accepted);
    }
}
