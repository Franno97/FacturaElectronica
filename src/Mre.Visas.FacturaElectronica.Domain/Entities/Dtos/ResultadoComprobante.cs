using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Domain.Entities.Dtos
{
  public class ResultadoComprobante
  {
    public ResultadoFactura entidad { get; set; }
    public List<string> errores { get; set; }
    
  }
  public class ResultadoFactura
  {
    public ResultadoFactura()
    {
      Numero = "";
      ClaveAcceso = "";
      FechaAutorizacion = "";
      FirmadoPor = "";
      FechaEmision = "";
    }
    public string Numero { get; set; }
    public string ClaveAcceso { get; set; }
    public string FechaAutorizacion { get; set; }
    public string FirmadoPor { get; set; }
    public string FechaEmision { get; set; }
    public bool TieneNotaCredito { get; set; }
  }
}
