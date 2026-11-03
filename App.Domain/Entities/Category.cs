using App.Domain.Entities.Common;

namespace App.Domain.Entities
{
    public class Category: BaseEntity<int>, IAuditEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Product>? Products { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
