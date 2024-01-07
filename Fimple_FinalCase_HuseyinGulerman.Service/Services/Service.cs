using AutoMapper;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Result.Abstract;
using Fimple_FinalCase_HuseyinGulerman.Core.Result.Concrete;
using Fimple_FinalCase_HuseyinGulerman.Core.Services;
using Fimple_FinalCase_HuseyinGulerman.Core.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fimple_FinalCase_HuseyinGulerman.Service.Exceptions;

namespace Fimple_FinalCase_HuseyinGulerman.Service.Services
{
    public class Service<TEntity, TRequest, TResponse> : IService<TEntity, TRequest, TResponse> where TEntity : BaseEntity where TRequest : class where TResponse : class
    {
        protected readonly IUnitOfWork _uow;
        protected readonly IMapper _mapper;
        public Service(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<IAppResult<TResponse>> AddAsync(TRequest request)
        {
            var newEntity = _mapper.Map<TEntity>(request);
            await _uow.GetRepository<TEntity>().AddAsync(newEntity);
            await _uow.CommitAsync();

            var newResponse = _mapper.Map<TResponse>(newEntity);
            return AppResult<TResponse>.Success(StatusCodes.Status200OK, newResponse);
        }

        public async Task<IAppResult<IEnumerable<TResponse>>> AddRangeAsync(IEnumerable<TRequest> requests)
        {
            var newEntities = _mapper.Map<IEnumerable<TEntity>>(requests);
            await _uow.GetRepository<TEntity>().AddRangeAsync(newEntities);
            await _uow.CommitAsync();

            var newResponses = _mapper.Map<IEnumerable<TResponse>>(newEntities);
            return AppResult<IEnumerable<TResponse>>.Success(StatusCodes.Status200OK, newResponses);
        }

        public async Task<IAppResult<bool>> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            var anyEntity = await _uow.GetRepository<TEntity>().AnyAsync(expression);

            return AppResult<bool>.Success(StatusCodes.Status200OK, anyEntity);
        }

        public async Task<IAppResult<IEnumerable<TResponse>>> GetAllActiveAsync()
        {
            var entities = await _uow.GetRepository<TEntity>().GetAllActive().ToListAsync();
            var responseEntities = _mapper.Map<IEnumerable<TResponse>>(entities);

            return AppResult<IEnumerable<TResponse>>.Success(StatusCodes.Status200OK, responseEntities);
        }

        public async Task<IAppResult<IEnumerable<TResponse>>> GetAllAsync()
        {
            var entities = await _uow.GetRepository<TEntity>().GetAll().ToListAsync();
            var responseEntities = _mapper.Map<IEnumerable<TResponse>>(entities);

            return AppResult<IEnumerable<TResponse>>.Success(StatusCodes.Status200OK, responseEntities);
        }

        public async Task<IAppResult<IEnumerable<TResponse>>> GetAllByExpAsync(params Expression<Func<TEntity, bool>>[] expression)
        {
            var entities = await _uow.GetRepository<TEntity>().GetAllAsync(expression).ToListAsync();
            var dtos = _mapper.Map<IEnumerable<TResponse>>(entities);

            return AppResult<IEnumerable<TResponse>>.Success(StatusCodes.Status200OK, dtos);
        }

        public async Task<IAppResult<IEnumerable<TResponse>>> GetAllByIncludeAsync(Expression<Func<TEntity, bool>> exp, params Expression<Func<TEntity, object>>[] includes)
        {
            var entities = await _uow.GetRepository<TEntity>().GetAllByIncludeAsync(exp, includes).ToListAsync();
            var dtos = _mapper.Map<IEnumerable<TResponse>>(entities);
            return AppResult<IEnumerable<TResponse>>.Success(StatusCodes.Status200OK, dtos);
        }

        public async Task<IAppResult<IEnumerable<TResponse>>> GetAllByIncludeParametersAsync(Expression<Func<TEntity, object>> include, params Expression<Func<TEntity, bool>>[] exps)
        {
            var entities = await _uow.GetRepository<TEntity>().GetAllByIncludeParametersAsync(include, exps).ToListAsync();
            var dtos = _mapper.Map<IEnumerable<TResponse>>(entities);

            return AppResult<IEnumerable<TResponse>>.Success(StatusCodes.Status200OK, dtos);
        }

        public async Task<IAppResult<TResponse>> GetByIdAsync(int id)
        {
            var entity = await _uow.GetRepository<TEntity>().GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"{typeof(TEntity).Name}({id}) does not exist");

            var response = _mapper.Map<TResponse>(entity);

            return AppResult<TResponse>.Success(StatusCodes.Status200OK, response);
        }

     

        public async Task<IAppResult<NoContentDTO>> RemoveAsync(int id)
        {
            var entity = await _uow.GetRepository<TEntity>().GetByIdAsync(id);
            _uow.GetRepository<TEntity>().Remove(entity);
            await _uow.CommitAsync();
            return AppResult<NoContentDTO>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<IAppResult<NoContentDTO>> RemoveRangeAsync(IEnumerable<int> ids)
        {

            var entities = await _uow.GetRepository<TEntity>().Where(x => ids.Contains(x.Id)).ToListAsync();
            _uow.GetRepository<TEntity>().RemoveRange(entities);
            await _uow.CommitAsync();
            return AppResult<NoContentDTO>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<IAppResult<TResponse>> UpdateAsync(TRequest request)
        {
            var entity = _mapper.Map<TEntity>(request);
            _uow.GetRepository<TEntity>().Update(entity);
            await _uow.CommitAsync();

            var tresponse = _mapper.Map<TResponse>(entity);
            return AppResult<TResponse>.Success(StatusCodes.Status204NoContent,tresponse);
        }

        public async Task<IAppResult<IEnumerable<TResponse>>> Where(Expression<Func<TEntity, bool>> expression)
        {
            var entities = await _uow.GetRepository<TEntity>().Where(expression).ToListAsync();
            var responses = _mapper.Map<IEnumerable<TResponse>>(entities);

            return AppResult<IEnumerable<TResponse>>.Success(StatusCodes.Status200OK, responses);
        }
    }
}
