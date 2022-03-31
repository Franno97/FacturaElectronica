using System;
using System.Collections.Generic;

namespace Mre.Visas.FacturaElectronica.Domain.Entities
{
  public class Factura : AuditableEntity
  {
    #region Constructors

    public Factura()
    {
      Created = DateTime.Now;
      LastModified = DateTime.Now;
      LastModifierId = Guid.NewGuid(); //TODO: Preguntar u obtener los datos de los usuarios
      CreatorId = Guid.NewGuid();//TODO: Preguntar u obtener los datos de los usuarios
      IsDeleted = false;
    }

    #endregion Constructors

    #region Properties

    public string CodigoUsuario { get; set; }
    public string CodigoOficina { get; set; }
    public string TipoIdentificacionComprador { get; set; }
    public string RazonSocialComprador { get; set; }
    public string IdentificacionComprador { get; set; }
    public string DireccionComprador { get; set; }
    public string TelefonoComprador { get; set; }
    public string CorreoComprador { get; set; }
    public decimal TotalSinImpuestos { get; set; }
    public decimal TotalDescuento { get; set; }
    public decimal ImporteTotal { get; set; }
    public string FechaEmisionLocal { get; set; }
    public string NumeroTramite { get; set; }
    public string Referencia { get; set; }
    public string Numero { get; set; }
    public string ClaveAcceso { get; set; }

    /// <summary>
    /// Resultado almacena
    /// Proceso OK
    /// Errores
    /// </summary>
    public string Resultado { get; set; }

    /// <summary>
    /// Estado del proceso
    /// 1 = enviado
    /// 2 = procesado al proveedor
    /// </summary>
    public string EstadoProceso { get; set; }


    public List<FacturaDetalle> FacturaDetalle { get; set; }

    public List<FacturaPago> FacturaPago { get; set; }

    #endregion Properties
  }
}