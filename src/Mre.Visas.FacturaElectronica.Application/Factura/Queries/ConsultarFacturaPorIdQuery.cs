using Mre.Visas.FacturaElectronica.Application.Factura.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
  public class ConsultarFacturaPorIdQuery : ConsultarFacturaPorIdRequest, IRequest<ApiResponseWrapper>
  {
    public ConsultarFacturaPorIdQuery(ConsultarFacturaPorIdRequest request)
    {
      Id = request.Id;
    }

    public class ConsultarTrámitePorIdQueryHandler : BaseHandler, IRequestHandler<ConsultarFacturaPorIdQuery, ApiResponseWrapper>
    {
      public ConsultarTrámitePorIdQueryHandler(IUnitOfWork unitOfWork)
          : base(unitOfWork)
      {
      }

      public async Task<ApiResponseWrapper> Handle(ConsultarFacturaPorIdQuery query, CancellationToken cancellationToken)
      {
        var response = new ApiResponseWrapper();
        var factura = await UnitOfWork.FacturaRepository.GetByIdAsync(query.Id);
        if (factura == null)
          response = new ApiResponseWrapper(HttpStatusCode.NotFound, null);
        else
          response = new ApiResponseWrapper(HttpStatusCode.OK, factura);

        return response;
      }
    }
  }

  public class ConsultarFacturaPorIdQueryValidator : AbstractValidator<ConsultarFacturaPorIdQuery>
  {
    public ConsultarFacturaPorIdQueryValidator()
    {
      RuleFor(e => e.Id)
          .NotEmpty().WithMessage("{PropertyName} es requerdio.")
          .NotNull().WithMessage("{PropertyName} no puede ser nulo.");
    }
  }
}