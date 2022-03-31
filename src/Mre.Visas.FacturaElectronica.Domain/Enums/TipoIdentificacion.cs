namespace Mre.Visas.FacturaElectronica.Domain.Enums
{
  public class TipoIdentificacion
  {
    #region Constructors

    public TipoIdentificacion()
    {
    }

    #endregion Constructors

    #region Attributes

    public enum Tipo
    { RUC, CEDULA, PASAPORTE, IDENTIFICACION_EXTERIOR }

    /// <summary>
    /// RUC = 04
    /// </summary>
    public const string RUC = "04";

    /// <summary>
    /// CEDULA = 05
    /// </summary>
    public const string CEDULA = "05";

    /// <summary>
    /// PASAPORTE 06
    /// </summary>
    public const string PASAPORTE = "06";

    /// <summary>
    /// IDENTIFICACION DEL EXTERIOR = 08
    /// </summary>
    public const string IDENTIFICACION_EXTERIOR = "08";

    #endregion Attributes

    public static string GetValor(string tipo)
    {
      string resultado = string.Empty;
      switch (tipo)
      {
        case "RUC":
          resultado = RUC;
          break;

        case "CEDULA":
          resultado = CEDULA;
          break;

        case "PASAPORTE":
          resultado = PASAPORTE;
          break;

        case "IDENTIFICACION_EXTERIOR":
          resultado = IDENTIFICACION_EXTERIOR;
          break;
      }

      //return resultado;
      return "06";
    }
  }
}