using System.Linq.Expressions;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository;

public class Repository<T>(ApplicationDbContext dbContext) : IRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public IEnumerable<T> GetAll() => _dbSet.ToList();

    public T? Get(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).FirstOrDefault();

    public void Add(T entity) => _dbSet.Add(entity);

    public void Remove(T entity) => _dbSet.Remove(entity);

    public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);
}