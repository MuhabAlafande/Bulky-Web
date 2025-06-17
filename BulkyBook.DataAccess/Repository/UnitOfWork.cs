using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.DataAccess.Repository;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public ICategoryRepository CategoryRepository { get; private set; } = new CategoryRepository(dbContext);
    public IProductRepository ProductRepository { get; private set; } = new ProductRepository(dbContext);

    public void Save() => dbContext.SaveChanges();
}