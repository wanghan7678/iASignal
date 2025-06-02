using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace iASignalApi.Repository;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition);
    void Create(T entity);
    void Update(T entity);

}

public abstract class StockRepositoryBase<T>(StockDbContext context) : IRepositoryBase<T> where T: class
{
    protected StockDbContext stockDbContext = context;
    
    public IQueryable<T> FindAll()
    {
        return stockDbContext.Set<T>().AsNoTracking();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition)
    {
        return stockDbContext.Set<T>().AsNoTracking().Where(condition);
    }

    public void Create(T entity)
    {
        stockDbContext.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        stockDbContext.Set<T>().Update(entity);
    }
}