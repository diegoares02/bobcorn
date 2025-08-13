using BobsCorn.Domain.Entities;

namespace BobsCorn.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetProductByIdAsync(int productId);
        Task DecrementProductQuantityAsync(int productId);
    }
}
