using Mre.Visas.FacturaElectronica.Application.Repositories;
using System;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Application.Factura.Repositories
{
  public interface IFacturaRepository : IRepository<Domain.Entities.Factura>
  {
    Task<Domain.Entities.Factura> GetByNumeroTramite(string numeroTramite, Guid IdArancel);
    
    Task<Domain.Entities.Factura> GetByClaveAcceso(string claveAcceso);
  }
}
