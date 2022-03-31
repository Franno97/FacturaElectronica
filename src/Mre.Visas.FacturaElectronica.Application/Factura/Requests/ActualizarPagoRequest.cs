using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Application.Factura.Requests
{
  public class ActualizarPagoRequest
  {
    public Guid idPagoDetalle { get; set; }
    public string ClaveAcceso { get; set; }
  }
}
