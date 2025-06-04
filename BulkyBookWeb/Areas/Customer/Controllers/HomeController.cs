using System.Diagnostics;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController(IUnitOfWork unitOfWork) : Controller {
    public IActionResult Index() {
        var productList = unitOfWork.ProductRepository.GetAll();
        return View(productList);
    }

    public IActionResult Details(int id) {
        var product = unitOfWork.ProductRepository.Get(product => product.Id == id);

        return View(product);
    }

    public IActionResult Privacy() {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}