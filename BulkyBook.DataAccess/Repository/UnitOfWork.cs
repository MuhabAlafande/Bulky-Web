using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.DataAccess.Repository;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public ICategoryRepository CategoryRepository { get; private set; } = new CategoryRepository(dbContext);
    public IProductRepository ProductRepository { get; private set; } = new ProductRepository(dbContext);
    public ICompanyRepository CompanyRepository { get; private set; } = new CompanyRepository(dbContext);
    public IShoppingCartRepository ShoppingCartRepository { get; private set; } = new ShoppingCartRepository(dbContext);
    public IApplicationUserRepository ApplicationUserRepository { get; private set; } = new ApplicationUserRepository(dbContext);
    public void Save() => dbContext.SaveChanges();
}