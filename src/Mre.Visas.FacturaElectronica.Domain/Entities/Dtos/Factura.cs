using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Domain.Entities.Dtos
{
  public class Factura
  {
    public string CodigoUsuario { get; set; } = "";
    public string CodigoOficina { get; set; } = "";

    public string TipoIdentificacionComprador { get; set; } = "";
    public string RazonSocialComprador { get; set; } = "";
    public string IdentificacionComprador { get; set; } = "";
    public string DireccionComprador { get; set; } = "";
    public string TelefonoComprador { get; set; } = "";
    public string CorreoComprador { get; set; } = "";

    public decimal TotalSinImpuestos { get; set; } = 0;
    public decimal TotalDescuento { get; set; } = 0;
    public decimal ImporteTotal { get; set; } = 0;
    public decimal Porcentaje { get; set; } = 0;

    public string FechaEmisionLocal { get; set; } //yyyyMMdd
    public string Referencia { get; set; } = "";
    public string Origen { get; set; } = "";

    public string NumeroTramite { get; set; } = "";
    public string DescripcionGeneral { get; set; } = "";

    public List<FacturaDetalle> FacturaDetalle { get; set; } = new List<FacturaDetalle>();
    public List<FacturaPagos> FacturaPagos { get; set; } = new List<FacturaPagos>();
  }

  public class FacturaDetalle
  {
    public FacturaDetalle()
    {
      OrdenDetalle = 0;
      CodigoPrincipal = "";
      CodigoAuxiliar = "";
      Descripcion = "";
      Cantidad = 0;
      PrecioUnitario = 0;
      Descuento = 0;
      PrecioTotalSinImpuesto = 0;
      CampoAdicional1Nombre = "";
      CampoAdicional1Valor = "";
      CampoAdicional2Nombre = "";
      CampoAdicional2Valor = "";
      CampoAdicional3Nombre = "";
      CampoAdicional3Valor = "";
      IdArancel = Guid.Empty;
    }

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

  public class FacturaPagos
  {
    public FacturaPagos()
    {
      Orden = 0;
      FormaPago = 0;
      Total = 0;
      IdPagoDetalle = Guid.Empty;
    }
    public int Orden { get; set; }
    public int FormaPago { get; set; }
    public decimal Total { get; set; }
    public Guid IdPagoDetalle { get; set; }
  }
}
