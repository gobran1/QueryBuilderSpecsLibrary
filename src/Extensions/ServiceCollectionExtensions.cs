using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QueryBuilderSpecs.Interfaces;
using QueryBuilderSpecs.Repositories;
using QueryBuilderSpecs.WorkManager;

namespace QueryBuilderSpecs.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseQueryBuilderSpecs(
            this IServiceCollection services,
            Type? customUOWType = null,
            Type? customRepoType = null
        )
        {
            services.AddScoped(typeof(IGenericRepository<,>), customRepoType ?? typeof(GenericRepository<,>));

            services.AddScoped(typeof(IUnitOfWork<>), customUOWType ?? typeof(UnitOfWork<>));

            return services;
        }

        public static IServiceCollection AddFilterBuilder<TEntity, TFilter, TFilterBuilder>(
            this IServiceCollection services)
            where TEntity : class
            where TFilter : class
            where TFilterBuilder : class, IGenericFiltersBuilder<TEntity, TFilter>
        {
            services.AddScoped<IGenericFiltersBuilder<TEntity, TFilter>, TFilterBuilder>();

            return services;
        }
    }
}