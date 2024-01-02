using Fimple_FinalCase_HuseyinGulerman.Core.DTOs;
using Fimple_FinalCase_HuseyinGulerman.Core.Interfaces;
using Fimple_FinalCase_HuseyinGulerman.Core.Result.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.Services
{
    public interface IService<TEntity, TRequest, TResponse> where TEntity : IEntity where TRequest : class where TResponse : class
    {
        Task<IAppResult<TResponse>> AddAsync(TRequest request);
        Task<IAppResult<IEnumerable<TResponse>>> AddRangeAsync(IEnumerable<TRequest> requests);
        Task<IAppResult<NoContentDTO>> UpdateAsync(TRequest request);
        Task<IAppResult<NoContentDTO>> RemoveAsync(int id);
        Task<IAppResult<NoContentDTO>> RemoveRangeAsync(IEnumerable<int> ids);
        Task<IAppResult<IEnumerable<TResponse>>> Where(Expression<Func<TEntity, bool>> expression);
        Task<IAppResult<TResponse>> GetByIdAsync(int id);
        Task<IAppResult<IEnumerable<TResponse>>> GetAllAsync();
        Task<IAppResult<IEnumerable<TResponse>>> GetAllActiveAsync();
        Task<IAppResult<IEnumerable<TResponse>>> GetAllByIncludeAsync(Expression<Func<TEntity, bool>> exp, params Expression<Func<TEntity, object>>[] includes);
        Task<IAppResult<bool>> AnyAsync(Expression<Func<TEntity, bool>> expression); Task<IAppResult<IEnumerable<TResponse>>> GetAllByIncludeParametersAsync(Expression<Func<TEntity, object>> include, params Expression<Func<TEntity, bool>>[] exps);

    }
}
