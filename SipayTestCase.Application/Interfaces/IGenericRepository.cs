using System.Linq.Expressions;
using SipayTestCase.Domain.Entities;

namespace SipayTestCase.Application.Interfaces;

public interface IGenericRepository<TEntity> where TEntity: BaseEntity
{
    Task<TEntity?> GetAsync(Guid id);
    Task<TEntity?> GetAsync(Guid id, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity?> GetAsync(Expression<Func<TEntity?, bool>> filter = null);
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null);
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes);
    Task<List<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> filter = null, string? orderColumn = null, string? orderType = null, int page = 0, int pageSize = 0);
    Task<long> CountAsync(Expression<Func<TEntity, bool>> filter = null);
    Task InsertAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}