using Mre.Visas.FacturaElectronica.Application.Factura.Repositories;
using Mre.Visas.FacturaElectronica.Infrastructure.Persistence.Contexts;
using Mre.Visas.FacturaElectronica.Infrastructure.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Infrastructure.Factura.Repositories
{
public class FacturaPagoRepository : Repository<Domain.Entities.FacturaPago>, IFacturaPagoRepository
  {
    #region Constructors

    public FacturaPagoRepository(ApplicationDbContext context) : base(context)
    {

    }

    #endregion Constructors
  }
}

