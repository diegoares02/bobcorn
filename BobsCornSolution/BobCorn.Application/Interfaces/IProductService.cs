using System.Net;
using BobsCorn.Application.DTOs;

namespace BobsCorn.Application.Interfaces
{
    public interface IProductService
    {
        Task<(HttpStatusCode, string)> BuyCornAsync(ProductBuyDto productBuyDto);

        Task<List<ReportDto>> GetPurchaseReportAsync(int userId);
    }
}
