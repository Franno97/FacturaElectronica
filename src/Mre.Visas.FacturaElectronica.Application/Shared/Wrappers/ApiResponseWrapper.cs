using System.Collections.Generic;
using System.Net;

namespace Mre.Visas.FacturaElectronica.Application.Wrappers
{
    public class ApiResponseWrapper
    {
        #region Constructors

        public ApiResponseWrapper()
        {
        }

        public ApiResponseWrapper(HttpStatusCode httpStatusCode, object result)
        {
            HttpStatusCode = httpStatusCode;
            Result = result;
        }

        public ApiResponseWrapper(HttpStatusCode httpStatusCode, string message)
        {
            HttpStatusCode = httpStatusCode;
            Message = message;
        }

        #endregion Constructors

        #region Properties

        public ICollection<string> Errors { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public string Message { get; set; }

        public object Result { get; set; }

        #endregion Properties
    }
}