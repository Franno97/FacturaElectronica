using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Application.Factura.Requests
{
  public class ConsultarFacturaPorNumeroTramiteRequest
  {
    public string NumeroTramite { get; set; }
    public Guid IdArancel { get; set; }
  }
}
