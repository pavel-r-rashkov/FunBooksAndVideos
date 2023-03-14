using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using FunBooksAndVideos.Application.Common.CollectionUtils;

namespace FunBooksAndVideos.Web.CollectionUtils;

public class PaginationBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Metadata.ModelType != typeof(Pagination))
        {
            return null;
        }

        return new BinderTypeModelBinder(typeof(PaginationBinder));
    }
}
