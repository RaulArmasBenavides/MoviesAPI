using ApiMovies.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApiMovies.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            return services;
        }
    }
}
