using Mre.Visas.FacturaElectronica.Application.Responses;
using System;

namespace Mre.Visas.FacturaElectronica.Application.Factura.Responses
{
  public class GrabarFacturaReponse 
  {

    public string Estado { get; set; }
    public string Mensaje { get; set; }
    public Guid Id { get; set; }
    public string ClaveAcceso { get; set; }
  }
}