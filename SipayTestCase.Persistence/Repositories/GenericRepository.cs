using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using SipayTestCase.Application.Interfaces;
using SipayTestCase.Domain.Entities;
using SipayTestCase.Persistence.Context;

namespace SipayTestCase.Persistence.Repositories;

public class  GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    public readonly ApplicationDbContext DbContext;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<TEntity?> GetAsync(Guid id) =>
        await DbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<TEntity?> GetAsync(Guid id, params Expression<Func<TEntity, object>>[] includes)
    {
        if (includes is null)
            return await DbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        var set = DbContext.Set<TEntity>();
        foreach (var include in includes)
            await set.Include(include).ToListAsync();

        return await set.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity?, bool>> filter = null)
    {
        if (filter == null)
        {
            throw new Exception("Filter cannot be null");
        }

        return await DbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filter);
    }
    
    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null)
    {
        if (filter != null)
        {
            return await DbContext.Set<TEntity>().Where(filter).AsNoTracking().ToListAsync();
        }

        return await DbContext.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = DbContext.Set<TEntity>();

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (filter != null)
        {
            return await query.Where(filter).AsNoTracking().ToListAsync();
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<List<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> filter = null,
        string? orderColumn = null, string? orderType = null, int page = 0, int pageSize = 0)
    {
        IQueryable<TEntity> query = DbContext.Set<TEntity>();

        if (!string.IsNullOrEmpty(orderColumn) && !string.IsNullOrEmpty(orderType))
        {
            query = query.OrderBy($"{orderColumn} {orderType}");
        }

        if (filter != null)
        {
            return await query.Where(filter).Skip(page * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
        }

        return await query.Skip(page * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
    }

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>> filter = null)
    {
        if (filter != null)
        {
            return await DbContext.Set<TEntity>().Where(filter).CountAsync();
        }

        return await DbContext.Set<TEntity>().CountAsync();
    }

    public async Task InsertAsync(TEntity entity)
    {
        await DbContext.Set<TEntity>().AddAsync(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
        await DbContext.SaveChangesAsync();
    }
}