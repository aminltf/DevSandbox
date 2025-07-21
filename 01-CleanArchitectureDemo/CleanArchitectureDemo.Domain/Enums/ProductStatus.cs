using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureDemo.Domain.Enums;

public enum ProductStatus
{
    [Display(Name = "Active")]
    Active = 1,

    [Display(Name = "Inactive")]
    Inactive = 2,

    [Display(Name = "OutOfStock")]
    OutOfStock = 3
}
