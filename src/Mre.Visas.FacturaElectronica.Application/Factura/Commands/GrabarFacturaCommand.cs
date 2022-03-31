using FluentValidation;
using MediatR;
using Mre.Visas.FacturaElectronica.Application.Factura.Requests;
using Mre.Visas.FacturaElectronica.Application.Shared.Handlers;
using Mre.Visas.FacturaElectronica.Application.Shared.Interfaces;
using Mre.Visas.FacturaElectronica.Application.Wrappers;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Application.Factura.Commands
{
  public class GrabarFacturaCommand : GrabarFacturaRequest, IRequest<ApiResponseWrapper>
  {
    #region Constructors

    public GrabarFacturaCommand(GrabarFacturaRequest request)
    {
      factura = request.factura;
    }

    #endregion Constructors

    #region Handlers

    public class GrabarFacturaCommandHandler : BaseHandler, IRequestHandler<GrabarFacturaCommand, ApiResponseWrapper>
    {
      #region Constructors

      public GrabarFacturaCommandHandler(IUnitOfWork unitOfWork)
          : base(unitOfWork)
      {
      }

      #endregion Constructors

      #region Methods

      public async Task<ApiResponseWrapper> Handle(GrabarFacturaCommand command, CancellationToken cancellationToken)
      {
        var resultado = new ApiResponseWrapper();

        var factura = new Domain.Entities.Factura()
        {
          CodigoOficina = command.factura.CodigoOficina,
          CodigoUsuario = command.factura.CodigoUsuario,
          RazonSocialComprador = command.factura.RazonSocialComprador,
          TipoIdentificacionComprador = Domain.Enums.TipoIdentificacion.GetValor(command.factura.TipoIdentificacionComprador),
          IdentificacionComprador = command.factura.IdentificacionComprador,
          CorreoComprador = command.factura.CorreoComprador,
          TelefonoComprador = command.factura.TelefonoComprador,
          DireccionComprador = command.factura.DireccionComprador,
          FechaEmisionLocal = command.factura.FechaEmisionLocal,
          ImporteTotal = command.factura.ImporteTotal,
          TotalSinImpuestos = command.factura.TotalSinImpuestos,
          Referencia = command.factura.Referencia,
          NumeroTramite = command.factura.NumeroTramite,
          Resultado = "Grabado",
          Numero = string.Empty,
          ClaveAcceso = string.Empty,
          EstadoProceso = "1",
          //DescripcionGeneral = "Ejemplo de facturación eletrónica";
          //NumeroTramite = "1235";
          //Origen = "VisasBI";
          //Porcentaje = 0;
          //TotalDescuento = 0;
        };
        factura.AssignId();
        var facturaDetalles = new List<Domain.Entities.FacturaDetalle>();
        foreach (var item in command.factura.FacturaDetalle)
        {
          facturaDetalles.Add(new Domain.Entities.FacturaDetalle
          {
            FacturaId = factura.Id,
            OrdenDetalle = item.OrdenDetalle,
            CodigoPrincipal = item.CodigoPrincipal,
            CodigoAuxiliar = item.CodigoAuxiliar,
            Descripcion = item.Descripcion,
            Cantidad = item.Cantidad,
            PrecioUnitario = item.PrecioUnitario,
            Descuento = item.Descuento,
            PrecioTotalSinImpuesto = item.PrecioTotalSinImpuesto,
            CampoAdicional1Nombre = item.CampoAdicional1Nombre,
            CampoAdicional1Valor = item.CampoAdicional1Valor,
            CampoAdicional2Nombre = item.CampoAdicional2Nombre,
            CampoAdicional2Valor = item.CampoAdicional2Valor,
            CampoAdicional3Nombre = item.CampoAdicional3Nombre,
            CampoAdicional3Valor = item.CampoAdicional3Valor,
            IdArancel = item.IdArancel
          });
        }

        var facturaPagos = new List<Domain.Entities.FacturaPago>();

        foreach (var pago in command.factura.FacturaPagos)
        {
          facturaPagos.Add(new Domain.Entities.FacturaPago
          {
            FacturaId = factura.Id,
            Orden = pago.Orden,
            FormaPago = pago.FormaPago,
            Total = pago.Total,
            IdPagoDetalle = pago.IdPagoDetalle,
            Created = System.DateTime.Now,
            LastModified = System.DateTime.Now
          });
        }
        factura.FacturaDetalle = facturaDetalles;
        factura.FacturaPago = facturaPagos;
        await UnitOfWork.FacturaRepository.InsertAsync(factura).ConfigureAwait(false);

        await UnitOfWork.SaveChangesAsync().ConfigureAwait(false);

        resultado = new ApiResponseWrapper(HttpStatusCode.OK, factura.Id);

        return resultado;
      }

      #endregion Methods
    }

    #endregion Handlers
  }

  public class GrabarFacturaCommandValidator : AbstractValidator<GrabarFacturaCommand>
  {
    public GrabarFacturaCommandValidator()
    {
      RuleFor(e => e.factura.CodigoUsuario)
          .NotEmpty().WithMessage("{PropertyName} es requerido.")
          .NotNull().WithMessage("{PropertyName} no debe ser nulo.")
          .MaximumLength(300).WithMessage("{PropertyName} tamaño máximo 300 caracteres.");

      RuleFor(e => e.factura.CodigoOficina)
      .NotEmpty().WithMessage("{PropertyName} es requerido.")
      .NotNull().WithMessage("{PropertyName} no debe ser nulo.")
      .MaximumLength(20).WithMessage("{PropertyName} tamaño máximo 20 caracteres.");

      RuleFor(e => e.factura.TipoIdentificacionComprador)
         .IsEnumName(typeof(Domain.Enums.TipoIdentificacion.Tipo)).WithMessage("{PropertyName} Solo puede aplicar estos (RUC, CEDULA, PASAPORTE, IDENTIFICACION_EXTERIOR).")
         .MaximumLength(25).WithMessage("{PropertyName} tamaño máximo 25 caracteres.");

      RuleFor(e => e.factura.RazonSocialComprador)
      .NotEmpty().WithMessage("{PropertyName} es requerido.")
      .NotNull().WithMessage("{PropertyName} no debe ser nulo.")
      .MaximumLength(300).WithMessage("{PropertyName} tamaño máximo 300 caracteres.");

      RuleFor(e => e.factura.IdentificacionComprador)
      .NotEmpty().WithMessage("{PropertyName} es requerido.")
      .NotNull().WithMessage("{PropertyName} no debe ser nulo.")
      .MaximumLength(20).WithMessage("{PropertyName} tamaño máximo 20 caracteres.");

      RuleFor(e => e.factura.DireccionComprador)
      .NotEmpty().WithMessage("{PropertyName} es requerido.")
      .NotNull().WithMessage("{PropertyName} no debe ser nulo.")
      .MaximumLength(300).WithMessage("{PropertyName} tamaño máximo 300 caracteres.");

      RuleFor(e => e.factura.TelefonoComprador)
      .NotEmpty().WithMessage("{PropertyName} es requerido.")
      .NotNull().WithMessage("{PropertyName} no debe ser nulo.")
      .MaximumLength(300).WithMessage("{PropertyName} tamaño máximo 300 caracteres.");

      RuleFor(e => e.factura.CorreoComprador)
      .NotEmpty().WithMessage("{PropertyName} es requerido.")
      .NotNull().WithMessage("{PropertyName} no debe ser nulo.")
      .EmailAddress().WithMessage("{PropertyName} formato valido.")
      .MaximumLength(300).WithMessage("{PropertyName} tamaño máximo 300 caracteres.");

      RuleFor(e => e.factura.TotalSinImpuestos)
      .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

      RuleFor(e => e.factura.TotalDescuento)
      .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

      RuleFor(e => e.factura.ImporteTotal)
      .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

      RuleFor(e => e.factura.Porcentaje)
     .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

      RuleFor(e => e.factura.FechaEmisionLocal)
     .NotEmpty().WithMessage("{PropertyName} es requerido.")
     .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

      //.Equal(default(System.DateTime)).WithMessage("{PropertyName} no es el formato correcto decimal.");

      RuleFor(e => e.factura.Referencia)
     .NotEmpty().WithMessage("{PropertyName} es requerido.")
     .NotNull().WithMessage("{PropertyName} no debe ser nulo.")
     .MaximumLength(300).WithMessage("{PropertyName} tamaño máximo 300 caracteres.");

      RuleFor(e => e.factura.FacturaDetalle)
     .NotEmpty().WithMessage("{PropertyName} es requerido.")
     .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

      RuleForEach(e => e.factura.FacturaDetalle).ChildRules(detalle =>
      {
        detalle.RuleFor(x => x.OrdenDetalle)
          .NotEmpty().WithMessage("{PropertyName} es requerido.")
          .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

        detalle.RuleFor(x => x.CodigoPrincipal)
          .NotEmpty().WithMessage("{PropertyName} es requerido.")
          .NotNull().WithMessage("{PropertyName} no debe ser nulo.")
          .MaximumLength(25).WithMessage("{PropertyName} tamaño máximo 25 caracteres.");

        detalle.RuleFor(x => x.CodigoAuxiliar)
          .MaximumLength(25).WithMessage("{PropertyName} tamaño máximo 25 caracteres.");

        detalle.RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull().WithMessage("{PropertyName} no debe ser nulo.")
            .MaximumLength(300).WithMessage("{PropertyName} tamaño máximo 300 caracteres.");

        detalle.RuleFor(x => x.Cantidad)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

        detalle.RuleFor(x => x.PrecioUnitario)
            //.NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

        detalle.RuleFor(x => x.Descuento)
        .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

        detalle.RuleFor(x => x.PrecioTotalSinImpuesto)
        //.NotEmpty().WithMessage("{PropertyName} es requerido.")
        .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

        detalle.RuleFor(x => x.CampoAdicional1Nombre)
        .MaximumLength(300).WithMessage("{PropertyName} tamaño máximo 300 caracteres.");

        detalle.RuleFor(x => x.CampoAdicional1Valor)
        .MaximumLength(300).WithMessage("{PropertyName} tamaño máximo 300 caracteres.");

        detalle.RuleFor(x => x.CampoAdicional2Nombre)
        .MaximumLength(300).WithMessage("{PropertyName} tamaño máximo 300 caracteres.");

        detalle.RuleFor(x => x.CampoAdicional2Valor)
        .MaximumLength(300).WithMessage("{PropertyName} tamaño máximo 300 caracteres.");

        detalle.RuleFor(x => x.CampoAdicional3Nombre)
        .MaximumLength(300).WithMessage("{PropertyName} tamaño máximo 300 caracteres.");

        detalle.RuleFor(x => x.CampoAdicional3Valor)
        .MaximumLength(300).WithMessage("{PropertyName} tamaño máximo 300 caracteres.");

      });


      RuleFor(e => e.factura.FacturaPagos)
      .NotEmpty().WithMessage("{PropertyName} es requerido.")
      .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

      RuleForEach(e => e.factura.FacturaPagos).ChildRules(pago =>
      {
        pago.RuleFor(x => x.Orden)
          .NotEmpty().WithMessage("{PropertyName} es requerido.")
          .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

        pago.RuleFor(x => x.FormaPago)
          .NotEmpty().WithMessage("{PropertyName} es requerido.")
          .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

        pago.RuleFor(x => x.Total)
          //.NotEmpty().WithMessage("{PropertyName} es requerido.")
          .NotNull().WithMessage("{PropertyName} no debe ser nulo.");
      });

    }
  }
}