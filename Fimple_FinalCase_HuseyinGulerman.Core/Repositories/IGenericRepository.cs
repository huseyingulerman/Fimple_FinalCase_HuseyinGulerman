using Fimple_FinalCase_HuseyinGulerman.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.Repositories
{
   public interface IGenericRepository<T> where T : class, IEntity
    {
        Task<T> GetByIdAsync(int id);
        
        IQueryable<T> GetAll();
        IQueryable<T> GetAllActive();
        IQueryable<T> GetAllByIncludeParametersAsync(Expression<Func<T, object>> include, params Expression<Func<T, bool>>[] exps);
        IQueryable<T> GetAllByIncludeAsync(Expression<Func<T, bool>> exp, params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAllAsync(params Expression<Func<T, bool>>[] exps);
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task<bool> Activate(int id);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task AddRangeAsync(IEnumerable<T> entities);
        bool Update(T entity);
        bool Remove(T entity);
        bool RemoveRange(IEnumerable<T> entities);
    }
}
