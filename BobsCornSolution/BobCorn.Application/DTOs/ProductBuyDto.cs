using System.ComponentModel.DataAnnotations;

namespace BobsCorn.Application.DTOs
{
    public class ProductBuyDto
    {
        public int ProductId { get; set; }
        public string Email { get; set; }
    }
}
