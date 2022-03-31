using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Mre.Visas.FacturaElectronica.Application.Wrappers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Api.Middlewares
{
    public class ApiExceptionMiddleware
    {
        #region Constructors

        public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        #endregion Constructors

        #region Attributes

        private readonly ILogger<ApiExceptionMiddleware> _logger;

        private readonly RequestDelegate _next;

        #endregion Attributes

        #region Methods

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var responseModel = new ApiResponseWrapper();

                switch (ex)
                {
                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.HttpStatusCode = HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors.Select(e => e.ErrorMessage).ToList();
                        break;

                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.HttpStatusCode = HttpStatusCode.NotFound;
                        responseModel.Errors = new List<string> { e.InnerException is null ? e.Message : e.InnerException.Message };
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.HttpStatusCode = HttpStatusCode.InternalServerError;
                        responseModel.Errors = new List<string> { ex.InnerException is null ? ex.Message : ex.InnerException.Message };
                        break;
                }

                _logger.LogError(ex, ex.InnerException is null ? ex.Message : ex.InnerException.Message);

                await response.WriteAsync(JsonConvert.SerializeObject(responseModel, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })).ConfigureAwait(false);
            }
        }

        #endregion Methods
    }
}