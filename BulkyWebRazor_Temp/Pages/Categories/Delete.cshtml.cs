using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories;

[BindProperties]
public class Delete(ApplicationDbContext dbContext) : PageModel
{
    public Category Category { get; set; }

    public IActionResult OnGet(int? id)
    {
        if (id is null or 0) return NotFound();

        var category = dbContext.Categories.Find(id);
        if (category is null) return NotFound();

        Category = category;
        return Page();
    }

    public IActionResult OnPost()
    {
        dbContext.Categories.Remove(Category);
        dbContext.SaveChanges();
        TempData["success"] = "Category deleted successfully.";
        return RedirectToPage("Index");
    }
}