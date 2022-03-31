using Mre.Visas.FacturaElectronica.Infrastructure.Shared.Repositories;
using Mre.Visas.FacturaElectronica.Application.Factura.Repositories;
using Mre.Visas.FacturaElectronica.Infrastructure.Persistence.Contexts;

namespace Mre.Visas.FacturaElectronica.Infrastructure.Factura.Repositories
{
  public class FacturaDetalleRepository : Repository<Domain.Entities.FacturaDetalle>, IFacturaDetalleRepository
  {
    #region Constructors

    public FacturaDetalleRepository(ApplicationDbContext context) : base(context)
    {

    }

    #endregion Constructors
  }
}

