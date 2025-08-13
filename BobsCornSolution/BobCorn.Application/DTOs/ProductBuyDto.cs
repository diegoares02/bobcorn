using System.ComponentModel.DataAnnotations;

namespace BobsCorn.Application.DTOs
{
    public class ProductBuyDto
    {
        [Required(ErrorMessage = "A ProductId is required to make a purchase.")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "A UserId is required to make a purchase.")]
        public int UserId { get; set; }
    }
}
