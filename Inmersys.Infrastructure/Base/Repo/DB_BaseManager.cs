using Inmersys.Domain.DB;
using Inmersys.Domain.DB.Base;
using Inmersys.Domain.DB.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Inmersys.Infrastructure.Base.Repo
{
    public class DB_BaseManager
    {
        private readonly CRM_DB _db;

        public DB_BaseManager(CRM_DB db)
        {
            _db = db;
        }

        public virtual async Task<bool> ExistsAsync<TEntity>(ulong id)
            where TEntity : BaseEntity
        {
            TEntity? entity = await _db.Set<TEntity>().FindAsync(id);

            return entity != null;
        }

        public virtual async Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> func)
            where TEntity : BaseEntity
        {
            ICollection<TEntity>? entities = await _db.Set<TEntity>().Where(func).ToListAsync();

            return entities.Any();
        }

        public virtual async Task<TEntity?> FindAsync<TEntity>(ulong id, bool include_all = false)
            where TEntity : BaseEntity
        {
            return include_all ? await _db.Set<TEntity>().Include(_db.GetThreePath(typeof(TEntity))).FirstOrDefaultAsync(ent => ent.id == id) : await _db.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<TEntity?> FindAsync<TEntity>(Expression<Func<TEntity, bool>> func, bool include_all = false)
            where TEntity : BaseEntity
        {
            return include_all ? await _db.Set<TEntity>().Include(_db.GetThreePath(typeof(TEntity))).FirstOrDefaultAsync(func) : await _db.Set<TEntity>().FirstOrDefaultAsync(func);
        }

        public virtual async Task<ICollection<TEntity>> ListAsync<TEntity>(bool include_all = false)
            where TEntity : BaseEntity
        {
            return include_all ? await _db.Set<TEntity>().Include(_db.GetThreePath(typeof(TEntity))).ToListAsync() : await _db.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<ICollection<TEntity>> ListAsync<TEntity>(Expression<Func<TEntity, bool>> func,bool include_all = false)
            where TEntity : BaseEntity
        {
            return include_all ? await _db.Set<TEntity>().Where(func).Include(_db.GetThreePath(typeof(TEntity))).ToListAsync() : await _db.Set<TEntity>().Where(func).ToListAsync();
        }

        public virtual async Task RemoveAsync<TEntity>(TEntity entity)
            where TEntity : BaseEntity
        {
            _db.Entry(entity).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync<TEntity>(TEntity entity)
            where TEntity : BaseEntity
        {
            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public virtual async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : BaseEntity
        {
            await _db.AddAsync(entity);
        }
    }
}
