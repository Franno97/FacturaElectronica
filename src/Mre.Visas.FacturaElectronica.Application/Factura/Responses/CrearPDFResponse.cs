using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Application.Factura.Responses
{
  public class CrearPDFResponse
  {
    public byte[] Entidad { get; set; }
    public List<string> Errores { get; set; }
    
  }
}
