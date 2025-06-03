using Bulky.DataAccess.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers;

public class CategoryController(ApplicationDbContext dbContext) : Controller {
    public IActionResult Index() {
        var categories = dbContext.Categories.ToList();
        return View(categories);
    }

    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category category) {
        if (category.Name == category.DisplayOrder.ToString()) {
            ModelState.AddModelError("name", "Display Order cannot be the same as Name.");
        }

        if (category.Name == "test") {
            ModelState.AddModelError("", "Test is an invalid value.");
        }

        if (ModelState.IsValid) {
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();
            TempData["success"] = "Category created successfully.";
            return RedirectToAction("Index", "Category");
        }

        return View();
    }

    public IActionResult Edit(int? id) {
        if (id is null or 0) return NoContent();

        var category = dbContext.Categories.Find(id);
        if (category == null) return NotFound();

        return View(category);
    }

    [HttpPost]
    public IActionResult Edit(Category category) {
        if (!ModelState.IsValid) return View();
        dbContext.Categories.Update(category);
        dbContext.SaveChanges();
        TempData["success"] = "Category updated successfully.";
        return RedirectToAction("Index", "Category");
    }

    public IActionResult Delete(int? id) {
        if (id is null or 0) return NoContent();

        var category = dbContext.Categories.Find(id);
        if (category == null) return NotFound();

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id) {
        if (id is null or 0) return NotFound();
        var category = dbContext.Categories.Find(id);
        if (category == null) return NotFound();
        dbContext.Categories.Remove(category);
        dbContext.SaveChanges();
        TempData["success"] = "Category deleted successfully.";

        return RedirectToAction("Index", "Category");
    }
}