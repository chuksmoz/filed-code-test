using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FiledCode.Domain.Interfaces
{
    public interface IAsyncRepository<T>
    {
        Task<T> GetByIdAsync(long id);
        //Task<T> GetByAsync(ISpecification<T> spec);

        Task<T> GetByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeExpressions);

        Task<T> GetFirstAsync();
        Task<T> GetFirstAsync(params Expression<Func<T, object>>[] includeExpressions);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAllAsync(params Expression<Func<T, object>>[] includeExpressions);
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeExpressions);
        //Task<T> AddAsync(T entity);
        void Add(T entity);
        //Task UpdateAsync(T entity);
        void Update(T entity);
        //Task DeleteAsync(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
    }

}
