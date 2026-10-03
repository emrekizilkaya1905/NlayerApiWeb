namespace App.Repositories.Products;

public class Product
{
    public int Id { get; set; }
    //Best practice is for name desribe how many characters are allowed in the string.
    public string Name { get; set; } = default!;
    //Best practice is for price to be decimal type because it is more precise than float or double.
    public decimal Price { get; set; }
    public int Stock { get; set; }

}

