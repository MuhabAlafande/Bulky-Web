using System.Security.Claims;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize]
public class CartController(IUnitOfWork unitOfWork) : Controller
{
    private ShoppingCartViewModel ShoppingCartViewModel { get; set; }

    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        var shoppingCartList = unitOfWork.ShoppingCartRepository.GetAll(cart => cart.ApplicationUserId == userId).ToList();

        CalculateCartsPrice(shoppingCartList);

        ShoppingCartViewModel = new ShoppingCartViewModel
        {
            ShoppingCartList = shoppingCartList,
            OrderTotal = CalculateOrderTotal(shoppingCartList)
        };

        return View(ShoppingCartViewModel);
    }

    public IActionResult Plus(int cartId)
    {
        var shoppingCart = unitOfWork.ShoppingCartRepository.Get(cart => cart.Id == cartId);
        if (shoppingCart != null)
        {
            shoppingCart.Count++;
            unitOfWork.ShoppingCartRepository.Update(shoppingCart);
            unitOfWork.Save();
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Minus(int cartId)
    {
        var shoppingCart = unitOfWork.ShoppingCartRepository.Get(cart => cart.Id == cartId);
        if (shoppingCart != null)
            if (shoppingCart.Count <= 1) unitOfWork.ShoppingCartRepository.Remove(shoppingCart);
            else
            {
                shoppingCart.Count--;
                unitOfWork.ShoppingCartRepository.Update(shoppingCart);
            }

        unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove(int cartId)
    {
        var shoppingCart = unitOfWork.ShoppingCartRepository.Get(cart => cart.Id == cartId);
        if (shoppingCart != null)
        {
            unitOfWork.ShoppingCartRepository.Remove(shoppingCart);
            unitOfWork.Save();
        }

        return RedirectToAction(nameof(Index));
    }


    private void CalculateCartsPrice(List<ShoppingCart> shoppingCartList) => shoppingCartList.ForEach(cart => cart.Price =
        GetPriceBasedOnQuantity(cart.Product, cart.Count));

    private double CalculateOrderTotal(List<ShoppingCart> shoppingCartList) => shoppingCartList.Sum(cart => cart.Price * cart.Count);

    private double GetPriceBasedOnQuantity(Product product, int quantity) => quantity switch
    {
        <= 50 => product.Price,
        <= 100 => product.Price50,
        _ => product.Price100
    };

    public IActionResult Summary()
    {
        return View(ShoppingCartViewModel);
    }
}