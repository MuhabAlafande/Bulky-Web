using System.Linq.Expressions;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository;

public class ProductRepository(ApplicationDbContext dbContext) : Repository<Product>(dbContext), IProductRepository {
    private readonly ApplicationDbContext _dbContext = dbContext;

    public new IEnumerable<Product> GetAll() => _dbContext.Products.Include(p => p.Category).ToList();

    public new Product? Get(Expression<Func<Product, bool>> predicate) =>
        _dbContext.Products.Include(p => p.Category).FirstOrDefault(predicate);

    public void Update(Product product) {
        var productFromDb = _dbContext.Products.FirstOrDefault(p => p.Id == product.Id);
        if (productFromDb == null) return;

        productFromDb.Title = product.Title;
        productFromDb.Description = product.Description;
        productFromDb.Price = product.Price;
        productFromDb.ListPrice = product.ListPrice;
        productFromDb.Price50 = product.Price50;
        productFromDb.Price100 = product.Price100;
        productFromDb.CategoryId = product.CategoryId;
        productFromDb.Author = product.Author;
        if (product.ImageUrl != null) productFromDb.ImageUrl = product.ImageUrl;
    }
}