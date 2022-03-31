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
  public class ConsultarFacturaPorClaveAccesoQuery : ConsultarFacturaPorClaveAccesoRequest, IRequest<ApiResponseWrapper>
  {
    public ConsultarFacturaPorClaveAccesoQuery(ConsultarFacturaPorClaveAccesoRequest request)
    {
      ClaveAcceso = request.ClaveAcceso;
    }

    public class ConsultarTrámitePorIdQueryHandler : BaseHandler, IRequestHandler<ConsultarFacturaPorClaveAccesoQuery, ApiResponseWrapper>
    {
      public ConsultarTrámitePorIdQueryHandler(IUnitOfWork unitOfWork)
          : base(unitOfWork)
      {
      }

      public async Task<ApiResponseWrapper> Handle(ConsultarFacturaPorClaveAccesoQuery query, CancellationToken cancellationToken)
      {
        var response = new ApiResponseWrapper();
        var factura = await UnitOfWork.FacturaRepository.GetByClaveAcceso(query.ClaveAcceso);
        if (factura == null)
          response = new ApiResponseWrapper(HttpStatusCode.NotFound, null);
        else
          response = new ApiResponseWrapper(HttpStatusCode.OK, factura);

        return response;
      }
    }
  }

  public class ConsultarFacturaPorClaveAccesoQueryValidator : AbstractValidator<ConsultarFacturaPorClaveAccesoQuery>
  {
    public ConsultarFacturaPorClaveAccesoQueryValidator()
    {
      RuleFor(e => e.ClaveAcceso)
          .NotEmpty().WithMessage("{PropertyName} es requerdio.")
          .NotNull().WithMessage("{PropertyName} no puede ser nulo.");
    }
  }
}