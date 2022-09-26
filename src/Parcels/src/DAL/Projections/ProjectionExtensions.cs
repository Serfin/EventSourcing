using Microsoft.Extensions.DependencyInjection;

namespace Parcels;

public static class ProjectionExtensions
{
    public static void AddProjection<TEntity, TProjection>(this IServiceCollection services)
        where TEntity : class
        where TProjection : class, IProjection<TEntity>
    {
        services.AddTransient<IProjection<TEntity>, TProjection>();
    }
}
