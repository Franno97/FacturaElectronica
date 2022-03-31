using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Mre.Visas.FacturaElectronica.Application.Factura.Commands;
using Mre.Visas.FacturaElectronica.Application.Factura.Requests;
using Mre.Visas.FacturaElectronica.Api.Controllers;
using Newtonsoft.Json;
using System.Text;
using Mre.Visas.FacturaElectronica.Application.Factura.Queries;
using System.Net;
using Microsoft.Extensions.Configuration;
using Mre.Visas.FacturaElectronica.Application.Factura.Responses;
using Microsoft.Extensions.Options;
using Mre.Visas.FacturaElectronica.Application;
//using AutoMapper.Configuration;

namespace Mre.Visas.FacturaElectronica.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FacturaElectronicaController : BaseController
  {
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IConfiguration configuration;
    private readonly RemoteServices remoteServices;
    public FacturaElectronicaController(IHttpClientFactory ihttpClientFactory, IConfiguration iconfiguration, IOptions<RemoteServices> remoteServices)
    {
      httpClientFactory = ihttpClientFactory;
      configuration = iconfiguration;
      this.remoteServices = remoteServices.Value;
    }


    // POST: api/Multa/GrabarFactura
    [HttpPost("GrabarFactura")]
    [ActionName(nameof(GrabarFacturaAsync))]
    public async Task<IActionResult> GrabarFacturaAsync(GrabarFacturaRequest request)
    {
      var response = new GrabarFacturaResultadoResponse();

      #region ConexionServicio
      var httpClient = httpClientFactory.CreateClient("API_IDP");
      var disco = await httpClient.GetDiscoveryDocumentAsync();
      if (disco.IsError)
      {
        response = new GrabarFacturaResultadoResponse { Estado = "ERROR", Mensaje = "Problemas al acceder al endpoint discovery.\n" + disco.Exception };
        return BadRequest(response);
      }

      var clientId = "mre-terceros-client";
      var clientSecret = "45c38f9c-0810-46af-a818-a60a598c1647";
      var scope = "mre_facturacion.fullaccess mre_fe.fullaccess";
      var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(
          new ClientCredentialsTokenRequest
          {
            Address = disco.TokenEndpoint,
            ClientId = clientId,
            ClientSecret = clientSecret,
            Scope = scope
          });

      if (tokenResponse.IsError)
      {
        response = new GrabarFacturaResultadoResponse { Estado = "ERROR", Mensaje = "Problemas al solicitar el token de acceso.\n" + tokenResponse.Exception };
        return BadRequest(response);
      }

      #endregion

      #region Grabado
      bool validacion = false;
      //validamos si existe el tramite
      var tramite = new ConsultarFacturaPorNumeroTramiteRequest();
      tramite.NumeroTramite = request.factura.NumeroTramite;
      tramite.IdArancel = request.factura.FacturaDetalle[0].IdArancel;
      var tramiteDato = await Mediator.Send(new ConsultarFacturaPorNumeroTramiteQuery(tramite)).ConfigureAwait(false);
      if (tramiteDato.HttpStatusCode == HttpStatusCode.NotFound)// es la primera vez que ingresa
      {
        validacion = true;
      }
      if (validacion)
      {
        var factura = CrearFactura(request);
        var client = httpClientFactory.CreateClient("API_FAC");
        client.SetBearerToken(tokenResponse.AccessToken);
        try
        {
          var json = JsonConvert.SerializeObject(factura);
          var data = new StringContent(json, Encoding.UTF8, "application/json");
          var AddFacturaResponse = await client.PostAsync(client.BaseAddress + "FacturacionTerceros/AddFactura", data);
          if (AddFacturaResponse.IsSuccessStatusCode)
          {
            var jsonResultadoComprobante = await AddFacturaResponse.Content.ReadAsStringAsync();
            var resultadoComprobante = new Domain.Entities.Dtos.ResultadoComprobante();
            resultadoComprobante = JsonConvert.DeserializeObject<Domain.Entities.Dtos.ResultadoComprobante>(jsonResultadoComprobante);

            if (resultadoComprobante.errores.Count == 0)
            {
              var resultado = Ok(await Mediator.Send(new GrabarFacturaCommand(request)).ConfigureAwait(false));
              if (resultado.StatusCode == Ok().StatusCode)
              {
                #region Grabar Factura
                Guid idFactura = Guid.Empty;
                Application.Wrappers.ApiResponseWrapper demo = (Application.Wrappers.ApiResponseWrapper)resultado.Value;
                idFactura = Guid.Parse(demo.Result.ToString());
                #endregion

                #region Actualizar Clave de Acceso
                var facturaActualizar = new ActualizarFacturaRequest
                {
                  Id = idFactura,
                  ClaveAcceso = resultadoComprobante.entidad.ClaveAcceso,
                  FechaActualizacion = DateTime.Now,
                  Numero = resultadoComprobante.entidad.Numero
                };
                var resultadoAtualizar = Ok(await Mediator.Send(new ActualizarFacturaCommand(facturaActualizar)).ConfigureAwait(false));
                #endregion

                #region Actualizar Pago Facturado
                //string urlPagos = configuration.GetSection("RemoteServices").GetSection("Pago").GetSection("BaseUrl").Value;
                var actualizarPagoRequest = new ActualizarPagoRequest { idPagoDetalle = request.factura.FacturaPagos[0].IdPagoDetalle, ClaveAcceso = resultadoComprobante.entidad.ClaveAcceso };

                HttpClient Client = new HttpClient();
                string Uri = remoteServices.pago.BaseUrl + "api/Pago/ActualizarPago";
                //
                var datas = JsonConvert.SerializeObject(actualizarPagoRequest);
                var content = new StringContent(datas, Encoding.UTF8, "application/json");
                var ResponseActualizarPago = Client.PostAsync(Uri, content).Result;
                if (ResponseActualizarPago.IsSuccessStatusCode)
                {
                  string PlacesJson = ResponseActualizarPago.Content.ReadAsStringAsync().Result;
                  ActualizarPagoResponse actualizarPagoResponse = new ActualizarPagoResponse();
                  actualizarPagoResponse = JsonConvert.DeserializeObject<ActualizarPagoResponse>(PlacesJson);
                  if (!actualizarPagoResponse.result.Estado.Equals("OK"))
                  {
                    response = new GrabarFacturaResultadoResponse { Estado = "ERROR", Mensaje = "Servicio de Pagos: " + actualizarPagoResponse.result.Mensaje };
                  }
                }
                response.ClaveAcceso = resultadoComprobante.entidad.ClaveAcceso;
                response.FechaEmision = resultadoComprobante.entidad.FechaEmision;
                response.Numero = resultadoComprobante.entidad.Numero;
                response.FechaAutorizacion = resultadoComprobante.entidad.FechaAutorizacion;
                #endregion
              }
              else
              {
                response = new GrabarFacturaResultadoResponse { Estado = "ERROR", Mensaje = "Revisar GrabarFacturaCommand" };
              }
            }
            else
            {
              response = new GrabarFacturaResultadoResponse { Estado = "ERROR", Mensaje = "Servicio de Facturación Electrónica Emisor:\n " };
              foreach (var item in resultadoComprobante.errores)
              {
                response.Mensaje += item;
              }
            }
          }
          else
          {
            response = new GrabarFacturaResultadoResponse { Estado = "ERROR", Mensaje = "StatusCode: " + AddFacturaResponse.StatusCode + "\n" + "FacturacionTerceros: " + AddFacturaResponse.Content.ReadAsStringAsync().Result };
          }
        }
        catch (Exception ex)
        {
          response = new GrabarFacturaResultadoResponse { Estado = "ERROR", Mensaje = "Excepción: " + ex.Message };
        }
      }
      else
      {
        response = new GrabarFacturaResultadoResponse { Estado = "OK", Mensaje = "Tramite con arancel ya facturado", ClaveAcceso = tramiteDato.Message.ToString() };
      }

      #endregion
      if (response.Estado.Equals("OK"))
        return Ok(response);
      else
        return NotFound(response);
    }
    private Domain.Entities.Dtos.Factura CrearFactura(GrabarFacturaRequest request)
    {
      var factura = new Domain.Entities.Dtos.Factura()
      {
        CodigoOficina = request.factura.CodigoOficina,
        CodigoUsuario = request.factura.CodigoUsuario,

        RazonSocialComprador = request.factura.RazonSocialComprador,
        TipoIdentificacionComprador = Domain.Enums.TipoIdentificacion.GetValor(request.factura.TipoIdentificacionComprador),
        IdentificacionComprador = request.factura.IdentificacionComprador,
        CorreoComprador = request.factura.CorreoComprador,
        TelefonoComprador = request.factura.TelefonoComprador,
        DireccionComprador = request.factura.DireccionComprador,
        DescripcionGeneral = request.factura.DescripcionGeneral,
        FechaEmisionLocal = request.factura.FechaEmisionLocal,
        NumeroTramite = request.factura.NumeroTramite,
        Origen = request.factura.Origen,
        Referencia = request.factura.Referencia,

        Porcentaje = request.factura.Porcentaje,
        TotalDescuento = request.factura.TotalDescuento,
        ImporteTotal = request.factura.ImporteTotal,
        TotalSinImpuestos = request.factura.TotalSinImpuestos
      };
      var facturaDetalle = new Domain.Entities.Dtos.FacturaDetalle()
      {
        OrdenDetalle = request.factura.FacturaDetalle[0].OrdenDetalle,
        CodigoPrincipal = request.factura.FacturaDetalle[0].CodigoPrincipal,
        CodigoAuxiliar = request.factura.FacturaDetalle[0].CodigoAuxiliar,
        Descripcion = request.factura.FacturaDetalle[0].Descripcion,
        Cantidad = request.factura.FacturaDetalle[0].Cantidad,
        PrecioUnitario = request.factura.FacturaDetalle[0].PrecioUnitario,
        Descuento = request.factura.FacturaDetalle[0].Descuento,
        PrecioTotalSinImpuesto = request.factura.FacturaDetalle[0].PrecioTotalSinImpuesto,
      };
      factura.FacturaDetalle.Add(facturaDetalle);

      var facturaPago = new Domain.Entities.Dtos.FacturaPagos()
      {
        Orden = request.factura.FacturaPagos[0].Orden,
        FormaPago = request.factura.FacturaPagos[0].FormaPago,
        Total = request.factura.FacturaPagos[0].Total
      };
      factura.FacturaPagos.Add(facturaPago);

      return factura;
    }

    // POST: api/Multa/CrearPDF
    [HttpPost("CrearPDF")]
    [ActionName(nameof(CrearPDFAsync))]
    public async Task<IActionResult> CrearPDFAsync(ConsultarFacturaPorClaveAccesoRequest request)
    {
      var response = new CrearPDFResultadoResponse { Estado = "OK", Mensaje = "Crear Pdf exitoso", Pdf = "JVBERi0xLjcNCiW1tbW1DQoxIDAgb2JqDQo8PC9UeXBlL0NhdGFsb2cvUGFnZXMgMiAwIFIvTGFuZyhlcy1FQykgL1N0cnVjdFRyZWVSb290IDEwIDAgUi9NYXJrSW5mbzw8L01hcmtlZCB0cnVlPj4vTWV0YWRhdGEgMjAgMCBSL1ZpZXdlclByZWZlcmVuY2VzIDIxIDAgUj4+DQplbmRvYmoNCjIgMCBvYmoNCjw8L1R5cGUvUGFnZXMvQ291bnQgMS9LaWRzWyAzIDAgUl0gPj4NCmVuZG9iag0KMyAwIG9iag0KPDwvVHlwZS9QYWdlL1BhcmVudCAyIDAgUi9SZXNvdXJjZXM8PC9Gb250PDwvRjEgNSAwIFI+Pi9FeHRHU3RhdGU8PC9HUzcgNyAwIFIvR1M4IDggMCBSPj4vUHJvY1NldFsvUERGL1RleHQvSW1hZ2VCL0ltYWdlQy9JbWFnZUldID4+L01lZGlhQm94WyAwIDAgNTk1LjQgODQxLjhdIC9Db250ZW50cyA0IDAgUi9Hcm91cDw8L1R5cGUvR3JvdXAvUy9UcmFuc3BhcmVuY3kvQ1MvRGV2aWNlUkdCPj4vVGFicy9TL1N0cnVjdFBhcmVudHMgMD4+DQplbmRvYmoNCjQgMCBvYmoNCjw8L0ZpbHRlci9GbGF0ZURlY29kZS9MZW5ndGggMTc2Pj4NCnN0cmVhbQ0KeJyljj0LwjAURffA+w93TISmSdq0EUqHflgUChULDuKgUDspWPH/G6VLZ992H4d7D8IOWRa25baCynMUVYknMSXV95xLDRTs2soYLtbSYRqIHVd4ECt6YuFGQ2v0N2LagwoazkplLNJESevQ3z3UHFKML9+K8ZfcnBpiJ94JxycR8/cgLL9eRKAjDv8zSgQRN0ac0e+I1X5tT+w/N+2xZOH2U5pNsJxC3Zb4AAdNN5ENCmVuZHN0cmVhbQ0KZW5kb2JqDQo1IDAgb2JqDQo8PC9UeXBlL0ZvbnQvU3VidHlwZS9UcnVlVHlwZS9OYW1lL0YxL0Jhc2VGb250L0JDREVFRStDYWxpYnJpL0VuY29kaW5nL1dpbkFuc2lFbmNvZGluZy9Gb250RGVzY3JpcHRvciA2IDAgUi9GaXJzdENoYXIgMzIvTGFzdENoYXIgMTE3L1dpZHRocyAxOCAwIFI+Pg0K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Z3RoIDMwODk+Pg0Kc3RyZWFtDQo8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/Pjx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IjMuMS03MDEiPgo8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPgo8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiAgeG1sbnM6cGRmPSJodHRwOi8vbnMuYWRvYmUuY29tL3BkZi8xLjMvIj4KPHBkZjpQcm9kdWNlcj5NaWNyb3NvZnTCriBXb3JkIHBhcmEgTWljcm9zb2Z0IDM2NTwvcGRmOlByb2R1Y2VyPjwvcmRmOkRlc2NyaXB0aW9uPgo8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiAgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIj4KPGRjOmNyZWF0b3I+PHJkZjpTZXE+PHJkZjpsaT5NYXJjbyBBeWFsYTwvcmRmOmxpPjwvcmRmOlNlcT48L2RjOmNyZWF0b3I+PC9yZGY6RGVzY3JpcHRpb24+CjxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiICB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iPgo8eG1wOkNyZWF0b3JUb29sPk1pY3Jvc29mdMKuIFdvcmQgcGFyYSBNaWNyb3NvZnQgMzY1PC94bXA6Q3JlYXRvclRvb2w+PHhtcDpDcmVhdGVEYXRlPjIwMjItMDMtMDdUMTI6MDg6NTUtMDU6MDA8L3htcDpDcmVhdGVEYXRlPjx4bXA6TW9kaWZ5RGF0ZT4yMDIyLTAzLTA3VDEyOjA4OjU1LTA1OjAwPC94bXA6TW9kaWZ5RGF0ZT48L3JkZjpEZXNjcmlwdGlvbj4KPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIj4KPHhtcE1NOkRvY3VtZW50SUQ+dXVpZDpFQzc0MzYxMS0yQjM1LTRFQzktQjQ4Ri0zREZFRkYxMUJDQ0M8L3htcE1NOkRvY3VtZW50SUQ+PHhtcE1NOkluc3RhbmNlSUQ+dXVpZDpFQzc0MzYxMS0yQjM1LTRFQzktQjQ4Ri0zREZFRkYxMUJDQ0M8L3htcE1NOkluc3RhbmNlSUQ+PC9yZGY6RGVzY3JpcHRpb24+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAo8L3JkZjpSREY+PC94OnhtcG1ldGE+PD94cGFja2V0IGVuZD0idyI/Pg0KZW5kc3RyZWFtDQplbmRvYmoNCjIxIDAgb2JqDQo8PC9EaXNwbGF5RG9jVGl0bGUgdHJ1ZT4+DQplbmRvYmoNCjIyIDAgb2JqDQo8PC9UeXBlL1hSZWYvU2l6ZSAyMi9XWyAxIDQgMl0gL1Jvb3QgMSAwIFIvSW5mbyA5IDAgUi9JRFs8MTEzNjc0RUMzNTJCQzk0RUI0OEYzREZFRkYxMUJDQ0M+PDExMzY3NEVDMzUyQkM5NEVCNDhGM0RGRUZGMTFCQ0NDPl0gL0ZpbHRlci9GbGF0ZURlY29kZS9MZW5ndGggODY+Pg0Kc3RyZWFtDQp4nGNgAIL//xmBpCADA4haBqHugSnGV2CK6QmYYu4BUyzVEGoDhHoKlAdrZ4JQzBCKBUKxQihGCAVVyQbUx/oXrJ29E0xxxIGpnEtgqtKMgQEAPFcMnQ0KZW5kc3RyZWFtDQplbmRvYmoNCnhyZWYNCjAgMjMNCjAwMDAwMDAwMTAgNjU1MzUgZg0KMDAwMDAwMDAxNyAwMDAwMCBuDQowMDAwMDAwMTY2IDAwMDAwIG4NCjAwMDAwMDAyMjIgMDAwMDAgbg0KMDAwMDAwMDQ5MCAwMDAwMCBuDQowMDAwMDAwNzQwIDAwMDAwIG4NCjAwMDAwMDA5MDggMDAwMDAgbg0KMDAwMDAwMTE0NyAwMDAwMCBuDQowMDAwMDAxMjAwIDAwMDAwIG4NCjAwMDAwMDEyNTMgMDAwMDAgbg0KMDAwMDAwMDAxMSA2NTUzNSBmDQowMDAwMDAwMDEyIDY1NTM1IGYNCjAwMDAwMDAwMTMgNjU1MzUgZg0KMDAwMDAwMDAxNCA2NTUzNSBmDQowMDAwMDAwMDE1IDY1NTM1IGYNCjAwMDAwMDAwMTYgNjU1MzUgZg0KMDAwMDAwMDAxNyA2NTUzNSBmDQowMDAwMDAwMDAwIDY1NTM1IGYNCjAwMDAwMDE5MjkgMDAwMDAgbg0KMDAwMDAwMjE0MiAwMDAwMCBuDQowMDAwMDI3ODU4IDAwMDAwIG4NCjAwMDAwMzEwMzAgMDAwMDAgbg0KMDAwMDAzMTA3NSAwMDAwMCBuDQp0cmFpbGVyDQo8PC9TaXplIDIzL1Jvb3QgMSAwIFIvSW5mbyA5IDAgUi9JRFs8MTEzNjc0RUMzNTJCQzk0RUI0OEYzREZFRkYxMUJDQ0M+PDExMzY3NEVDMzUyQkM5NEVCNDhGM0RGRUZGMTFCQ0NDPl0gPj4NCnN0YXJ0eHJlZg0KMzEzNjANCiUlRU9GDQp4cmVmDQowIDANCnRyYWlsZXINCjw8L1NpemUgMjMvUm9vdCAxIDAgUi9JbmZvIDkgMCBSL0lEWzwxMTM2NzRFQzM1MkJDOTRFQjQ4RjNERkVGRjExQkNDQz48MTEzNjc0RUMzNTJCQzk0RUI0OEYzREZFRkYxMUJDQ0M+XSAvUHJldiAzMTM2MC9YUmVmU3RtIDMxMDc1Pj4NCnN0YXJ0eHJlZg0KMzE5NzYNCiUlRU9G" };
      #region ConexionServicio
      var httpClient = httpClientFactory.CreateClient("API_IDP");
      var disco = await httpClient.GetDiscoveryDocumentAsync();
      if (disco.IsError)
      {
        response = new CrearPDFResultadoResponse { Estado = "ERROR", Mensaje = "Problemas al acceder al endpoint discovery.\n" + disco.Exception };
        return BadRequest(response);
      }

      var clientId = "mre-terceros-client";
      var clientSecret = "45c38f9c-0810-46af-a818-a60a598c1647";
      var scope = "mre_facturacion.fullaccess mre_fe.fullaccess";
      var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(
          new ClientCredentialsTokenRequest
          {
            Address = disco.TokenEndpoint,
            ClientId = clientId,
            ClientSecret = clientSecret,
            Scope = scope
          });

      if (tokenResponse.IsError)
      {
        response = new CrearPDFResultadoResponse { Estado = "ERROR", Mensaje = "Problemas al solicitar el token de acceso.\n" + tokenResponse.Exception };
        return BadRequest(response);
      }

      #endregion
      var client = httpClientFactory.CreateClient("API_FAC");
      client.SetBearerToken(tokenResponse.AccessToken);
      string Uri = client.BaseAddress + "FacturacionTerceros/DescargarArchivo/" + request.ClaveAcceso + ".pdf";
      HttpResponseMessage Response = await client.PostAsync(Uri, null);

      if (Response.IsSuccessStatusCode)
      {
        try
        {
          var result = Response.Content.ReadAsStreamAsync();
          byte[] bytePdf;
          System.IO.MemoryStream ms = new System.IO.MemoryStream();
          result.Result.CopyTo(ms);
          bytePdf = ms.ToArray();
          string pdfBase64 = Convert.ToBase64String(bytePdf, 0, bytePdf.Length);
          if (pdfBase64.Equals(string.Empty))
          {
            response = new CrearPDFResultadoResponse
            {
              Estado = "ERROR",
              Pdf = string.Empty,
              Mensaje = "PDF no existe para esta clave de acceso."
            };
          }
          else
          {
            response = new CrearPDFResultadoResponse
            {
              Estado = "OK",
              Pdf = pdfBase64,
              Mensaje = "PDF creado correctamente"
            };
          }
        }
        catch (Exception ex)
        {
          response.Pdf = "";
          response.Estado = "ERROR";
          response.Mensaje = ex.Message;
        }
      }
      else
      {
        response.Estado = "Error";
        response.Mensaje = $"{Response.ReasonPhrase} ==> {Response.RequestMessage}";
      }
      if (response.Estado.Equals("OK"))
        return Ok(response);
      else
        return NotFound(response);
    }

    [HttpPost("ObtenerFacturaPorClaveAcceso")]
    [ActionName(nameof(ObtenerFacturaPorClaveAccesoAsync))]
    public async Task<IActionResult> ObtenerFacturaPorClaveAccesoAsync(ConsultarFacturaPorClaveAccesoRequest request)
    {
      return Ok(await Mediator.Send(new ConsultarFacturaPorClaveAccesoQuery(request)).ConfigureAwait(false));
    }

    [HttpPost("ObtenerFacturaPorId")]
    [ActionName(nameof(ObtenerFacturaPorIdAsync))]
    public async Task<IActionResult> ObtenerFacturaPorIdAsync(ConsultarFacturaPorIdRequest request)
    {
      return Ok(await Mediator.Send(new ConsultarFacturaPorIdQuery(request)).ConfigureAwait(false));
    }

    [HttpPost("ObtenerFacturaPorNumeroTramite")]
    [ActionName(nameof(ObtenerFacturaPorNumeroTramiteAsync))]
    public async Task<IActionResult> ObtenerFacturaPorNumeroTramiteAsync(ConsultarFacturaPorNumeroTramiteRequest request)
    {
      return Ok(await Mediator.Send(new ConsultarFacturaPorNumeroTramiteQuery(request)).ConfigureAwait(false));
    }
  }


}
