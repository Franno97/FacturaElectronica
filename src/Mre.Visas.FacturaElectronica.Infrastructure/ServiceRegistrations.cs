using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mre.Visas.FacturaElectronica.Application.Factura.Repositories;
using Mre.Visas.FacturaElectronica.Application.Shared.Interfaces;
using Mre.Visas.FacturaElectronica.Infrastructure.Factura.Repositories;
using Mre.Visas.FacturaElectronica.Infrastructure.Persistence.Contexts;
using Mre.Visas.FacturaElectronica.Infrastructure.Shared.Interfaces;
using Mre.Visas.FacturaElectronica.Infrastructure.Shared.Repositories;
using Mre.Visas.FacturaElectronica.Application.Repositories;


namespace Mre.Visas.FacturaElectronica.Infrastructure
{
  public static class ServiceRegistrations
  {
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<ApplicationDbContext>(
          options => options.UseSqlServer(configuration.GetConnectionString("ApplicationDbContext"),
          options => options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

      services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

      services.AddTransient<IUnitOfWork, UnitOfWork>();

      services.AddTransient<IFacturaRepository, FacturaRepository>();
      services.AddTransient<IFacturaDetalleRepository, FacturaDetalleRepository>();
      services.AddTransient<IFacturaPagoRepository, FacturaPagoRepository>();
    }
  }
}
