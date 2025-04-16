using Gee.Core.Responses;
using System.Linq.Expressions;

namespace Gee.Core.Interfaces
{
    public interface IGenericCRUDInterface<T> where T : class
    {
        Task<Response<T>> CreateAsync(T request);
        Task<Response<T>> UpdateAsync(T request);
        Task<Response<T>> DeleteAsync(T request);
        Task<PaginatedFilterModel<T>> GetAllAsync(T request);
        Task<Response<T>> FindByIdAsync(T request);
        Task<Response<T>> FindByNameAsync(string Name);
        Task<Response<T>> GetByAsync(Expression<Func<T, bool>> Expression);
        Task<Response<T>> GetById(int Id);

    }
}
