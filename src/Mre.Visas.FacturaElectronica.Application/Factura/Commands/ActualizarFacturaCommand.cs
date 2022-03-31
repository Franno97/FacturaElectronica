using FluentValidation;
using MediatR;
using Mre.Visas.FacturaElectronica.Application.Factura.Requests;
using Mre.Visas.FacturaElectronica.Application.Factura.Responses;
using Mre.Visas.FacturaElectronica.Application.Shared.Handlers;
using Mre.Visas.FacturaElectronica.Application.Shared.Interfaces;
using Mre.Visas.FacturaElectronica.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Application.Factura.Commands
{
  public class ActualizarFacturaCommand : ActualizarFacturaRequest, IRequest<ApiResponseWrapper>
  {
    #region Constructors

    public ActualizarFacturaCommand(ActualizarFacturaRequest request)
    {
      Id = request.Id;
      ClaveAcceso = request.ClaveAcceso;
      Numero = request.Numero;
      FechaActualizacion = request.FechaActualizacion;
    }

    #endregion Constructors

    #region Handlers

    public class ActualizarFacturaCommandHandler : BaseHandler, IRequestHandler<ActualizarFacturaCommand, ApiResponseWrapper>
    {
      #region Constructors

      public ActualizarFacturaCommandHandler(IUnitOfWork unitOfWork)
          : base(unitOfWork)
      {
      }

      #endregion Constructors

      #region Methods

      public async Task<ApiResponseWrapper> Handle(ActualizarFacturaCommand command, CancellationToken cancellationToken)
      {
        var response = new ActualizarFacturaResponse();
        var resultado = new ApiResponseWrapper();

        var factura = await UnitOfWork.FacturaRepository.GetByIdAsync(command.Id);
        factura.Numero = command.Numero;
        factura.ClaveAcceso = command.ClaveAcceso;
        factura.LastModified = command.FechaActualizacion;

        var resultadoFactura = UnitOfWork.FacturaRepository.Update(factura);
        if (resultadoFactura.Item1)
        {
          var result2 = await UnitOfWork.SaveChangesAsync();
          if (result2.Item1)
          {
            await UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
            response = new ActualizarFacturaResponse { Estado = "OK", Mensaje = "No se logro confirmar el almacenamiento" };
          }
          else
            response = new ActualizarFacturaResponse { Estado = "ERROR", Mensaje = "No se logro confirmar el almacenamiento" };
        }
        else
        {
          response = new ActualizarFacturaResponse { Estado = "ERROR", Mensaje = "No se logro almacenar" };
        }
        if (response.Estado.Equals("OK"))
          resultado = new ApiResponseWrapper(HttpStatusCode.OK, response);
        else
          resultado = new ApiResponseWrapper(HttpStatusCode.BadRequest, response);
        
        return resultado;
      }

      #endregion Methods
    }

    #endregion Handlers
  }

  public class ActualizarFacturaCommandValidator : AbstractValidator<ActualizarFacturaCommand>
  {
    public ActualizarFacturaCommandValidator()
    {
      RuleFor(e => e.ClaveAcceso)
          .NotEmpty().WithMessage("{PropertyName} es requerido.")
          .NotNull().WithMessage("{PropertyName} no debe ser nulo.")
          .MaximumLength(300).WithMessage("{PropertyName} tamaño máximo 300 caracteres.");

      RuleFor(e => e.Id)
      .NotEmpty().WithMessage("{PropertyName} es requerido.")
      .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

      RuleFor(e => e.Numero)
     .NotEmpty().WithMessage("{PropertyName} es requerido.")
     .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

      RuleFor(e => e.FechaActualizacion)
      .NotEmpty().WithMessage("{PropertyName} es requerido.")
      .NotNull().WithMessage("{PropertyName} no debe ser nulo.");

    }
  }
}