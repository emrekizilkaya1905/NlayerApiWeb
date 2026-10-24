namespace App.Repositories
{
    public interface IAuditEntity
    {
        DateTime CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }  
    }
}
