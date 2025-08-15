using System.Net;
using BobsCorn.Application.Interfaces;
using BobsCorn.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BobsCornSolution.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("available")]
        public IActionResult AvailableProducts()
        {
            return Ok(new ApiResponse<object>((int)HttpStatusCode.OK, "There are available products", new { Quantity = _productService.GetAvailableProduct() }));
        }
    }
}
