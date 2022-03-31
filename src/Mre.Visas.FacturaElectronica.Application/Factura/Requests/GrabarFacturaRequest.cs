using Mre.Visas.FacturaElectronica.Application.Requests;

namespace Mre.Visas.FacturaElectronica.Application.Factura.Requests
{
  public class GrabarFacturaRequest 
  {
    public Domain.Entities.Dtos.Factura factura { get; set; }
  }
}