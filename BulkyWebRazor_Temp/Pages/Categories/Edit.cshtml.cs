using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories;

[BindProperties]
public class Edit(ApplicationDbContext dbContext) : PageModel {
    public Category Category { get; set; }

    public IActionResult OnGet(int? id) {
        if (id is null or 0) return NotFound();

        var category = dbContext.Categories.Find(id);
        if (category is null) return NotFound();

        Category = category;
        return Page();
    }

    public IActionResult OnPost(int id) {
        Category.Id = id;
        dbContext.Update(Category);
        dbContext.SaveChanges();
        TempData["success"] = "Category updated successfully.";
        return RedirectToPage("Index");
    }
}