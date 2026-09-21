using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SantaCruz.Domain.Service;
using SantaCruz.Domain.ViewModel.Page;
using SantaCruz.Domain.ViewModel.Product;

namespace SantaCruz.Order.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController(IProductService productService) : DetailedController
    {
        private readonly IProductService _productService = productService;

        [HttpGet]
        public Task<IActionResult> Get([FromQuery] PageViewModel pageViewModel, CancellationToken cancellationToken)=>
            Handle(()=>_productService.List(pageViewModel, cancellationToken), Ok);

    }
}
