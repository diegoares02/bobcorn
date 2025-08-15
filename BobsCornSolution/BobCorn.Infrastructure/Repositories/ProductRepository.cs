using BobsCorn.Application.Interfaces;
using BobsCorn.Infrastructure.Data;
using BobsCorn.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BobsCorn.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly BobCornDbContext _context;
        public ProductRepository(BobCornDbContext context)
        {
            _context = context;
        }
        public async Task DecrementProductQuantityAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null && product.Quantity > 0)
            {
                product.Quantity--;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
        }

        public int? GetAvailableProducts()
        {
            return _context.Products.FirstOrDefault(x => x.Name == "Corn")?.Quantity;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        }
    }
}
