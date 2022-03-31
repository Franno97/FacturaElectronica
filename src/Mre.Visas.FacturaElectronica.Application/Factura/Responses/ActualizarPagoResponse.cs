using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Application.Factura.Responses
{
  public class ActualizarPagoResponse
  {
    public ResultadoPagos result { get; set; }
  }
  public class ResultadoPagos
  {

    public string Estado { get; set; }
    public string Mensaje { get; set; }
  }
}
