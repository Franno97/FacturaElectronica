using Mre.Visas.FacturaElectronica.Infrastructure.Shared.Repositories;
using Mre.Visas.FacturaElectronica.Application.Factura.Repositories;
using Mre.Visas.FacturaElectronica.Infrastructure.Persistence.Contexts;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Mre.Visas.FacturaElectronica.Infrastructure.Factura.Repositories
{
  public class FacturaRepository : Repository<Domain.Entities.Factura>, IFacturaRepository
  {
    #region Constructors

    public FacturaRepository(ApplicationDbContext context) : base(context)
    {

    }

    public async Task<Domain.Entities.Factura> GetByClaveAcceso(string claveAcceso)
    {
      return await _context.Facturas
        .Include(x => x.FacturaDetalle)
        .Include(x => x.FacturaPago)
        .Where(x => x.ClaveAcceso.Equals(claveAcceso)).FirstOrDefaultAsync();
    }

    #endregion Constructors

    #region Metodos Others 
    public async Task<Domain.Entities.Factura> GetByNumeroTramite(string numeroTramite, Guid IdArancel)
    {
      return await _context.Facturas
        .Include(x=>x.FacturaDetalle)
        .Include(x=>x.FacturaPago)
        .Where(x => x.NumeroTramite.Equals(numeroTramite) && x.FacturaDetalle.Any(y => y.IdArancel.Equals(IdArancel))).FirstOrDefaultAsync();
    }
    #endregion
  }
}
