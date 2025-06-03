using System.Linq.Expressions;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository;

public class CategoryRepository(ApplicationDbContext dbContext) : Repository<Category>(dbContext), ICategoryRepository {
    private readonly ApplicationDbContext _dbContext = dbContext;

    public void Update(Category category) => _dbContext.Update(category);
}