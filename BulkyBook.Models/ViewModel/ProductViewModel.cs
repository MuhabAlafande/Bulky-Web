using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulky.Models.ViewModel;

public class ProductViewModel
{
    public Product Product { get; set; }

    [ValidateNever] public IEnumerable<SelectListItem> CategorySelectItems { get; set; }
}