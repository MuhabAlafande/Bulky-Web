using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModel;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
// [Authorize(Roles = Sd.RoleAdmin)]
public class ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : Controller
{
    public IActionResult Index()
    {
        var products = unitOfWork.ProductRepository.GetAll().ToList();
        return View(products);
    }

    public IActionResult Upsert(int? id)
    {
        ProductViewModel productViewModel = new()
        {
            Product = new Product(),
            CategorySelectItems = unitOfWork.CategoryRepository.GetAll().Select(category =>
                new SelectListItem { Text = category.Name, Value = category.Id.ToString() })
        };

        if (id is null or 0) return View(productViewModel);
        else
        {
            productViewModel.Product = unitOfWork.ProductRepository.Get(product => product.Id == id) ?? new Product();
            return View(productViewModel);
        }
    }

    [HttpPost]
    public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            var wwwrootPath = webHostEnvironment.WebRootPath;
            if (file != null)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var productPath = Path.Combine(wwwrootPath, @"images/product");

                if (!string.IsNullOrEmpty(productViewModel.Product.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwrootPath, productViewModel.Product.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                }

                var filePath = Path.Combine(productPath, filename);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);
                productViewModel.Product.ImageUrl = @"/images/product/" + filename;
            }

            if (productViewModel.Product.Id == 0)
            {
                unitOfWork.ProductRepository.Add(productViewModel.Product);
            }
            else
            {
                Console.WriteLine(file?.ToString() ?? "Null");
                unitOfWork.ProductRepository.Update(productViewModel.Product);
            }

            unitOfWork.Save();
            TempData["success"] = "Product created successfully.";
            return RedirectToAction("Index", "Product");
        }

        return View();
    }

    public IActionResult Delete(int? id)
    {
        if (id is null or 0) return NoContent();

        var product = unitOfWork.ProductRepository.Get(product => product.Id == id);
        if (product == null) return NotFound();

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        if (id is null or 0) return NotFound();
        var product = unitOfWork.ProductRepository.Get(product => product.Id == id);
        if (product == null) return NotFound();
        unitOfWork.ProductRepository.Remove(product);
        unitOfWork.Save();
        TempData["success"] = "Product deleted successfully.";

        return RedirectToAction("Index", "Product");
    }
}