using BobsCorn.Domain.Entities;

namespace BobsCorn.Application.Interfaces
{
    public interface IUserProductLogRepository
    {
        Task<DateTime?> GetLastPurchaseTimeAsync(int userId);
        Task AddLogAsync(UserProductLog log);
        Task<List<UserProductLog>> GetLogsByUserIdAsync(int userId);
    }
}
