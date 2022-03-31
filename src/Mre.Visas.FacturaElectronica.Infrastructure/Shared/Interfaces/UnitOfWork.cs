using Mre.Visas.FacturaElectronica.Application.Factura.Repositories;
using Mre.Visas.FacturaElectronica.Application.Shared.Interfaces;
using Mre.Visas.FacturaElectronica.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Infrastructure.Shared.Interfaces
{
  public class UnitOfWork : IUnitOfWork
  {
    #region Constructors

    public UnitOfWork(ApplicationDbContext context,
      IFacturaRepository facturaRepository,
      IFacturaDetalleRepository facturaDetalleRepository,
      IFacturaPagoRepository facturaPagoRepository
      )
    {
      _context = context;
      FacturaRepository = facturaRepository;
      FacturaDetalleRepository = facturaDetalleRepository;
      FacturaPagoRepository = facturaPagoRepository;
    }

    //public UnitOfWork(
    //    ApplicationDbContext context,
    //    IFacturaDetalleRepository facturaDetalleRepository)
    //{
    //  _context = context;
    //  FacturaDetalleRepository = facturaDetalleRepository;
    //}

    //public UnitOfWork(
    //    ApplicationDbContext context,
    //    IFacturaPagoRepository facturaPagoRepository)
    //{
    //  _context = context;
    //  FacturaPagoRepository = facturaPagoRepository;
    //}

    #endregion Constructors

    #region Attributes

    protected readonly ApplicationDbContext _context;

    #endregion Attributes

    #region Properties

    public IFacturaRepository FacturaRepository { get; }
    public IFacturaDetalleRepository FacturaDetalleRepository { get; }
    public IFacturaPagoRepository FacturaPagoRepository { get; }

    #endregion Properties

    #region Methods

    public async Task<(bool, string)> SaveChangesAsync()
    {
      try
      {
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return (true, null);
      }
      catch (Exception ex)
      {
        return (false, ex.InnerException is null ? ex.Message : ex.InnerException.Message);
      }
    }

    #endregion Methods
  }
}