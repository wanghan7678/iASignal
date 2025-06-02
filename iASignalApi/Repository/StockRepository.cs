using System.Linq.Expressions;
using iASignalApi.Models;
using iASignalApi.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace iASignalApi.Repository;

public interface IStockRepository
{
    Task<ICollection<StockPortfolio>> FindPortfoliosByUserIdAsync(string userId);
    Task<ICollection<StockPortfolio>> FindPortfolioByStockIdAsync(string stockId);
    
    StockDbContext StockDbContext { get; set; }
}

public class StockRepository(StockDbContext stockDbContext) : StockRepositoryBase<StockDbContext>(stockDbContext), IStockRepository
{
    public async Task<ICollection<StockPortfolio>> FindPortfoliosByUserIdAsync(string userId)
    {
        return await stockDbContext.StockPortfolios.AsNoTracking()
            .Include(e => e.Receivers)
            .Where(e => e.Receivers.Any(r => r.Id == userId))
            .ToListAsync();
    }

    public Task<ICollection<StockPortfolio>> FindPortfolioByStockIdAsync(string stockId)
    {
        throw new NotImplementedException();
    }

    public StockDbContext StockDbContext { get; set; } = stockDbContext;
}