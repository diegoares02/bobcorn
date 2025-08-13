using BobsCorn.Application.Interfaces;
using BobsCorn.Domain.Entities;
using BobsCorn.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BobsCorn.Infrastructure.Repositories
{
    public class UserProductLogRepository : IUserProductLogRepository
    {
        private readonly BobCornDbContext _context;

        public UserProductLogRepository(BobCornDbContext context)
        {
            _context = context;
        }

        public async Task<DateTime?> GetLastPurchaseTimeAsync(int userId)
        {
            return await _context.UserProductLogs
                .Where(log => log.UserId == userId)
                .OrderByDescending(log => log.PurchaseDate)
                .Select(log => log.PurchaseDate)
                .FirstOrDefaultAsync();
        }

        public async Task AddLogAsync(UserProductLog log)
        {
            _context.UserProductLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserProductLog>> GetLogsByUserIdAsync(int userId)
        {
            return await _context.UserProductLogs
                .Include(log => log.User)
                .Include(log => log.Product)
                .Where(log => log.UserId == userId)
                .OrderByDescending(log => log.PurchaseDate)
                .ToListAsync();
        }
    }
}
