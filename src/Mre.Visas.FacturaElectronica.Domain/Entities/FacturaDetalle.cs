using System;

namespace Mre.Visas.FacturaElectronica.Domain.Entities
{
  public class FacturaDetalle : AuditableEntity
  {
    public FacturaDetalle()
    {
      Id = Guid.NewGuid();
      Created = DateTime.Now;
      LastModified = DateTime.Now;
      LastModifierId = Guid.NewGuid(); //TODO: Preguntar u obtener los datos de los usuarios
      CreatorId = Guid.NewGuid();//TODO: Preguntar u obtener los datos de los usuarios
      IsDeleted = false;
    }

    public Guid FacturaId { get; set; }
    public Domain.Entities.Factura Factura { get; set; }
    public int OrdenDetalle { get; set; }
    public string CodigoPrincipal { get; set; }
    public string CodigoAuxiliar { get; set; }
    public string Descripcion { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
    public decimal PrecioTotalSinImpuesto { get; set; }
    public string CampoAdicional1Nombre { get; set; }
    public string CampoAdicional1Valor { get; set; }
    public string CampoAdicional2Nombre { get; set; }
    public string CampoAdicional2Valor { get; set; }
    public string CampoAdicional3Nombre { get; set; }
    public string CampoAdicional3Valor { get; set; }
    public Guid IdArancel { get; set; }
  }
}