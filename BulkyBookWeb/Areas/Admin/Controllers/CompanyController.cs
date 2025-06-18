using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
// [Authorize(Roles = Sd.RoleAdmin)]
public class CompanyController(IUnitOfWork unitOfWork) : Controller
{
    public IActionResult Index()
    {
        var companies = unitOfWork.CompanyRepository.GetAll().ToList();
        return View(companies);
    }

    public IActionResult Upsert(int? id)
    {
        if (id is null or 0) return View(new Company());
        else
        {
            var company = unitOfWork.CompanyRepository.Get(company => company.Id == id) ?? new Company();
            return View(company);
        }
    }

    [HttpPost]
    public IActionResult Upsert(Company company)
    {
        if (ModelState.IsValid)
        {
            if (company.Id == 0) unitOfWork.CompanyRepository.Add(company);
            else unitOfWork.CompanyRepository.Update(company);

            unitOfWork.Save();
            TempData["success"] = "Company created successfully.";
            return RedirectToAction("Index", "Company");
        }

        return View();
    }

    public IActionResult Delete(int? id)
    {
        if (id is null or 0) return NoContent();

        var company = unitOfWork.CompanyRepository.Get(company => company.Id == id);
        if (company == null) return NotFound();

        return View(company);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        if (id is null or 0) return NotFound();
        var company = unitOfWork.CompanyRepository.Get(company => company.Id == id);
        if (company == null) return NotFound();
        unitOfWork.CompanyRepository.Remove(company);
        unitOfWork.Save();
        TempData["success"] = "Company deleted successfully.";

        return RedirectToAction("Index", "Company");
    }
}