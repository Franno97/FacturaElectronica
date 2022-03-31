using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        #region Constructors

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        #endregion Constructors

        #region Properties

        private readonly IEnumerable<IValidator<TRequest>> _validators;

        #endregion Properties

        #region Handlers

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var validators = _validators.Select(e => e.ValidateAsync(new ValidationContext<TRequest>(request), cancellationToken));
                var validations = await Task.WhenAll(validators).ConfigureAwait(false);

                var failures = validations.SelectMany(e => e.Errors)
                    .Where(e => e != null)
                    .ToArray();
                if (failures.Any())
                {
                    throw new ValidationException(failures);
                }
            }

            return await next().ConfigureAwait(false);
        }

        #endregion Handlers
    }
}