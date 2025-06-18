using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController(IUnitOfWork unitOfWork) : Controller
{
    public IActionResult Index()
    {
        var productList = unitOfWork.ProductRepository.GetAll();
        return View(productList);
    }

    public IActionResult Details(int id)
    {
        var product = unitOfWork.ProductRepository.Get(product => product.Id == id);
        ShoppingCart shoppingCart = new()
            { Product = product, Count = 1, ProductId = product.Id };
        return View(shoppingCart);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Details(int productId, int count)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        var cartFromDatabase = unitOfWork.ShoppingCartRepository.Get(cart => cart.ProductId == productId && cart.ApplicationUserId == userId);

        if (cartFromDatabase == null)
            unitOfWork.ShoppingCartRepository.Add(new ShoppingCart { ProductId = productId, Count = count, ApplicationUserId = userId });
        else
        {
            cartFromDatabase.Count += count;
            unitOfWork.ShoppingCartRepository.Update(cartFromDatabase);
        }

        unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}