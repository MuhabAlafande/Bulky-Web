using System.Linq.Expressions;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository;

public class ShoppingCartRepository(ApplicationDbContext dbContext) : Repository<ShoppingCart>(dbContext), IShoppingCartRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public IEnumerable<ShoppingCart> GetAll(Expression<Func<ShoppingCart, bool>> predicate) =>
        _dbContext.ShoppingCarts.Where(predicate).Include(cart => cart.Product).ToList();

    public void Update(ShoppingCart shoppingCart) => _dbContext.Update(shoppingCart);
}