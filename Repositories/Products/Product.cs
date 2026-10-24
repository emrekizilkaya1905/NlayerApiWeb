using App.Repositories.Categories;

namespace App.Repositories.Products;

public class Product : IAuditEntity
{
    public int Id { get; set; }
    //Best practice is for name desribe how many characters are allowed in the string.
    public string Name { get; set; } = default!;
    //Best practice is for price to be decimal type because it is more precise than float or double.
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

}

