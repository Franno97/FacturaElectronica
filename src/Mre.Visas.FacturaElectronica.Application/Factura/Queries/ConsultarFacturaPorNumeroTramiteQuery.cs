using FluentValidation;
using MediatR;
using Mre.Visas.FacturaElectronica.Application.Factura.Requests;
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

namespace Mre.Visas.FacturaElectronica.Application.Factura.Queries
{
  public class ConsultarFacturaPorNumeroTramiteQuery : ConsultarFacturaPorNumeroTramiteRequest, IRequest<ApiResponseWrapper>
  {
    public ConsultarFacturaPorNumeroTramiteQuery(ConsultarFacturaPorNumeroTramiteRequest request)
    {
      NumeroTramite = request.NumeroTramite;
      IdArancel = request.IdArancel;
    }

    public class ConsultarTramitePorIdQueryHandler : BaseHandler, IRequestHandler<ConsultarFacturaPorNumeroTramiteQuery, ApiResponseWrapper>
    {
      public ConsultarTramitePorIdQueryHandler(IUnitOfWork unitOfWork)
          : base(unitOfWork)
      {
      }

      public async Task<ApiResponseWrapper> Handle(ConsultarFacturaPorNumeroTramiteQuery query, CancellationToken cancellationToken)
      {
        var response = new ApiResponseWrapper();
        var factura = await UnitOfWork.FacturaRepository.GetByNumeroTramite(query.NumeroTramite, query.IdArancel);
        if (factura == null)
          response = new ApiResponseWrapper(HttpStatusCode.NotFound, string.Empty);
        else
          response = new ApiResponseWrapper(HttpStatusCode.OK, factura.ClaveAcceso);

        return response;
      }
    }
  }

  public class ConsultarFacturaPorNumeroTramiteQueryValidator : AbstractValidator<ConsultarFacturaPorNumeroTramiteQuery>
  {
    public ConsultarFacturaPorNumeroTramiteQueryValidator()
    {
      RuleFor(e => e.NumeroTramite)
          .NotEmpty().WithMessage("{PropertyName} es requerdio.")
          .NotNull().WithMessage("{PropertyName} no puede ser nulo.");

      RuleFor(e => e.IdArancel)
          .NotEmpty().WithMessage("{PropertyName} es requerdio.")
          .NotNull().WithMessage("{PropertyName} no puede ser nulo.");
    }
  }
}