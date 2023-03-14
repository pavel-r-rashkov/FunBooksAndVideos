using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using FunBooksAndVideos.Application.Common.CollectionUtils;

namespace FunBooksAndVideos.Web.CollectionUtils;

public class SortOrderBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Metadata.ModelType != typeof(IEnumerable<SortOrder>))
        {
            return null;
        }

        return new BinderTypeModelBinder(typeof(SortOrderBinder));
    }
}
