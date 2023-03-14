using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Application.Common;

public interface IFunBooksAndVideosContext
{
    DbSet<T> GetDbSet<T>() where T : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
