using Microsoft.AspNetCore.Mvc.ModelBinding;
using FunBooksAndVideos.Application.Common.CollectionUtils;

namespace FunBooksAndVideos.Web.CollectionUtils;

public class PaginationBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var skipParam = bindingContext.HttpContext.Request.Query["skip"];
        var takeParam = bindingContext.HttpContext.Request.Query["take"];

        var skip = Pagination.DefaultSkip;

        if (skipParam.Any() && !int.TryParse(skipParam.FirstOrDefault(), out skip))
        {
            throw new CollectionBindingException("Skip is not an integer");
        }

        var take = Pagination.DefaultTake;

        if (takeParam.Any() && !int.TryParse(takeParam.FirstOrDefault(), out take))
        {
            throw new CollectionBindingException("Take is not an integer");
        }

        bindingContext.Result = ModelBindingResult.Success(new Pagination(skip, take));

        return Task.CompletedTask;
    }
}
