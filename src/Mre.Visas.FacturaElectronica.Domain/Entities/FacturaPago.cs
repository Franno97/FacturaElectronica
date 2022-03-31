using System;

namespace Mre.Visas.FacturaElectronica.Domain.Entities
{
  public class FacturaPago : AuditableEntity
  {
    public FacturaPago()
    {
      Created = DateTime.Now;
      LastModified = DateTime.Now;
      LastModifierId = Guid.NewGuid(); //TODO: Preguntar u obtener los datos de los usuarios
      CreatorId = Guid.NewGuid();//TODO: Preguntar u obtener los datos de los usuarios
      IsDeleted = false;
    }

    public Guid FacturaId { get; set; }
    public Domain.Entities.Factura Factura { get; set; }
    public int Orden { get; set; }
    public int FormaPago { get; set; }
    public decimal Total { get; set; }
    public Guid IdPagoDetalle { get; set; }
  }
}