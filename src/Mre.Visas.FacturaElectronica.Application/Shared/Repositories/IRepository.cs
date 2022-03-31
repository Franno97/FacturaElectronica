using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mre.Visas.FacturaElectronica.Application.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<(bool, string)> DeleteAsync(Guid id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        Task<(bool, string)> InsertAsync(T entity);

        Task<(bool, string)> InsertRangeAsync(IReadOnlyCollection<T> entities);

        (bool, string) Update(T entity);
    }
}