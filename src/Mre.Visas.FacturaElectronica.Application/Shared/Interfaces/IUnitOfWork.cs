using Mre.Visas.FacturaElectronica.Application.Factura.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Application.Shared.Interfaces
{
  public interface IUnitOfWork
  {
    IFacturaRepository FacturaRepository { get; }
    IFacturaDetalleRepository FacturaDetalleRepository { get; }
    IFacturaPagoRepository FacturaPagoRepository { get; }

    Task<(bool, string)> SaveChangesAsync();
  }
}
