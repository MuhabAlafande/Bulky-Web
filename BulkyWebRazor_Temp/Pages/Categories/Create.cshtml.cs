using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories;

[BindProperties]
public class Create(ApplicationDbContext dbContext) : PageModel
{
    public Category Category { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        dbContext.Add(Category);
        dbContext.SaveChanges();
        TempData["success"] = "Category created successfully.";
        return RedirectToPage($"Index");
    }
}