using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Application.Factura.Responses
{
  public class CrearPDFResultadoResponse
  {
    public string Estado { get; set; }
    public string Mensaje { get; set; }
    public string Pdf { get; set; }
  }
}
