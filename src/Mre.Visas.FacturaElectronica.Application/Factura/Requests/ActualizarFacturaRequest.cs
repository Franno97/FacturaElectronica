using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Application.Factura.Requests
{
  public class ActualizarFacturaRequest
  {
    public Guid Id { get; set; }
    public string Numero { get; set; }
    public string ClaveAcceso { get; set; }
    public DateTime FechaActualizacion { get; set; }
  }
}
