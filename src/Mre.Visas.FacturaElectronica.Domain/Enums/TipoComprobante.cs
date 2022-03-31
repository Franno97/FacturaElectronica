namespace Mre.Visas.FacturaElectronica.Domain.Enums
{
    /// <summary>
    /// Esta clase contiene los atributos validos
    /// </summary>
    public class TipoComprobante
    {
        #region Constructors

        public TipoComprobante()
        {
        }

        #endregion Constructors

        #region Attributes

        /// <summary>
        /// FACTURA = 01
        /// </summary>
        public const string FACTURA = "01";

        /// <summary>
        /// NOTA DE CREDITO = 04
        /// </summary>
        public const string NOTA_CREDITO = "04";

        #endregion Attributes
    }
}