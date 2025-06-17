using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Sd.RoleAdmin)]
public class CategoryController(IUnitOfWork unitOfWork) : Controller
{
    public IActionResult Index()
    {
        var categories = unitOfWork.CategoryRepository.GetAll().ToList();
        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category category)
    {
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "Display Order cannot be the same as Name.");
        }

        if (category.Name == "test")
        {
            ModelState.AddModelError("", "Test is an invalid value.");
        }

        if (ModelState.IsValid)
        {
            unitOfWork.CategoryRepository.Add(category);
            unitOfWork.Save();
            TempData["success"] = "Category created successfully.";
            return RedirectToAction("Index", "Category");
        }

        return View();
    }

    public IActionResult Edit(int? id)
    {
        if (id is null or 0) return NoContent();

        var category = unitOfWork.CategoryRepository.Get(category => category.Id == id);
        if (category == null) return NotFound();

        return View(category);
    }

    [HttpPost]
    public IActionResult Edit(Category category)
    {
        if (!ModelState.IsValid) return View();
        unitOfWork.CategoryRepository.Update(category);
        unitOfWork.Save();
        TempData["success"] = "Category updated successfully.";
        return RedirectToAction("Index", "Category");
    }

    public IActionResult Delete(int? id)
    {
        if (id is null or 0) return NoContent();

        var category = unitOfWork.CategoryRepository.Get(category => category.Id == id);
        if (category == null) return NotFound();

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        if (id is null or 0) return NotFound();
        var category = unitOfWork.CategoryRepository.Get(category => category.Id == id);
        if (category == null) return NotFound();
        unitOfWork.CategoryRepository.Remove(category);
        unitOfWork.Save();
        TempData["success"] = "Category deleted successfully.";

        return RedirectToAction("Index", "Category");
    }
}