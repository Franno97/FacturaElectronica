using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Mre.Visas.FacturaElectronica.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddSwaggerExtension(this IServiceCollection services, string title, string version)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = title,
                    Version = version
                });
            });
        }
    }
}