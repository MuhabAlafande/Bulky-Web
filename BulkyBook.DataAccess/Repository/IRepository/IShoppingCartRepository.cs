using System.Linq.Expressions;
using Bulky.Models;

namespace Bulky.DataAccess.Repository.IRepository;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    IEnumerable<ShoppingCart> GetAll(Expression<Func<ShoppingCart, bool>> predicate);
    void Update(ShoppingCart shoppingCart);
}