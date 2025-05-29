using QueryBuilderSpecs.Interfaces;
using QueryBuilderSpecs.Repositories;
using QueryBuilderSpecs.WorkManager;

namespace QueryBuilderSpecs.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection UseQuerySpecificationBuilder(this IServiceCollection services,
            Type? customUOWType = null,
            Type? CustomRepoType = null
        )
        {

            var uowType = customUOWType ?? typeof(UnitOfWork);
            var repoType = CustomRepoType ?? typeof(GenericRepository<>);



            services.AddScoped(typeof(IUnitOfWork), uowType);
            services.AddScoped(typeof(IGenericRepository<>), repoType);

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
