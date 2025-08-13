using System.Net;
using BobsCorn.Application.Interfaces;
using BobsCorn.Domain.Entities;
using BobsCorn.Application.DTOs;

namespace BobsCorn.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUserProductLogRepository _userProductLogRepository;

        private readonly IProductRepository _productRepository;

        public ProductService(IUserProductLogRepository userProductLogRepository, IProductRepository productRepository)
        {
            _userProductLogRepository = userProductLogRepository;
            _productRepository = productRepository;
        }

        public async Task<(HttpStatusCode, string)> BuyCornAsync(ProductBuyDto productBuyDto)
        {
            var lastPurchase = await _userProductLogRepository.GetLastPurchaseTimeAsync(productBuyDto.UserId);

            if (lastPurchase.HasValue && lastPurchase.Value.AddMinutes(1) > DateTime.UtcNow)
            {
                return (HttpStatusCode.TooManyRequests, "You can only buy one corn per minute.");
            }

            var corn = await _productRepository.GetProductByIdAsync(productBuyDto.ProductId);
            if (corn == null || corn.Quantity <= 0)
            {
                return (HttpStatusCode.NotFound, "Corn is currently out of stock.");
            }

            var newPurchase = new UserProductLog
            {
                UserId = productBuyDto.UserId,
                ProductId = productBuyDto.ProductId,
                PurchaseDate = DateTime.UtcNow
            };

            await _userProductLogRepository.AddLogAsync(newPurchase);
            await _productRepository.DecrementProductQuantityAsync(productBuyDto.ProductId);

            return (HttpStatusCode.OK, "Success");
        }

        public async Task<List<ReportDto>> GetPurchaseReportAsync(int userId)
        {
            var userLogs = await _userProductLogRepository.GetLogsByUserIdAsync(userId);

            var report = userLogs.Select(log => new ReportDto
            {
                Email = log.User.Email,
                ProductName = log.Product.Name,
                PurchaseDateTime = log.PurchaseDate.ToString("yyyy-MM-dd HH:mm:ss"),
            }).ToList();

            return report;
        }
    }
}
