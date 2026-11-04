using App.Application;
using App.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanApp.API.Filters
{
    public class NotFoundFilter<T, TId>(IGenericRepository<T, TId> repository) : Attribute, IAsyncActionFilter where T : class where TId : struct
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var idValue = context.ActionArguments.TryGetValue("id", out var idAsObject) ? idAsObject : null;

            if (idAsObject is not TId id)
            {
                await next();
                return;
            }

            if (await repository.AnyAsync(id))
            {
                await next();
                return;
            }
            var entityName = typeof(T).Name;
            // action method name
            var actionName = context.ActionDescriptor.RouteValues["action"];

            var result = ServiceResult.Fail($"Data could not find.({entityName} with id {id} not found in action {actionName})");
            context.Result = new NotFoundObjectResult(result);
            return;
        }
    }
}
