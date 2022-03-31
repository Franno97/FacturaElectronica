namespace Mre.Visas.FacturaElectronica.Domain.Enums
{
    public class TipoFormaPago
    {
        #region Constructors

        public TipoFormaPago()
        {
        }

        #endregion Constructors

        #region Attributes

        /// <summary>
        /// COMPENSACION_DE_DEUDAS = 15
        /// </summary>
        public const string COMPENSACION_DE_DEUDAS = "15";

        /// <summary>
        /// TARJETA_DEBITO = 16
        /// </summary>
        public const string TARJETA_DEBITO = "16";

        /// <summary>
        /// DINERO_ELECTRONICO = 17
        /// </summary>
        public const string DINERO_ELECTRONICO = "17";

        /// <summary>
        /// TARJETA_PREPAGO = 18
        /// </summary>
        public const string TARJETA_PREPAGO = "18";

        /// <summary>
        /// TARJETA_CREDITO = 19
        /// </summary>
        public const string TARJETA_CREDITO = "19";

        /// <summary>
        /// OTROS_CON_UTILIZACION_DEL_SISTEMA_FINANCIERO = 20
        /// </summary>
        public const string OTROS_CON_UTILIZACION_DEL_SISTEMA_FINANCIERO = "20";

        /// <summary>
        /// ENDOSO DE TITULOS = 21
        /// </summary>
        public const string ENDOSO_TITULOS = "21";

        #endregion Attributes
    }
}