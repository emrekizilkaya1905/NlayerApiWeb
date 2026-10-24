using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace App.Repositories.Interceptors
{
    public class AuditDbContextinterceptor : SaveChangesInterceptor
    {
        public static readonly Dictionary<EntityState,Action<DbContext, IAuditEntity>> Behaviors = new()
        {
            { EntityState.Added,AddBehavior },
            { EntityState.Modified, UpdateBehavior }
        };
        private static void AddBehavior(DbContext context, IAuditEntity auditEntity)
        {
           auditEntity.CreatedDate = DateTime.UtcNow;
           context.Entry(auditEntity).Property(x => x.UpdatedDate).IsModified = false;
        }
        private static void UpdateBehavior(DbContext context, IAuditEntity auditEntity)
        {
            auditEntity.UpdatedDate = DateTime.UtcNow;
            context.Entry(auditEntity).Property(x => x.CreatedDate).IsModified = false;
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {

            foreach (var entry in eventData.Context!.ChangeTracker.Entries())
            {
                if (entry.Entity is not IAuditEntity auditEntity) continue;

                Behaviors[entry.State](eventData.Context, auditEntity);
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
