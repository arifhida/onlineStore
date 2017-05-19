using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OnlineStore.Data.Abstract;
using OnlineStore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineStore.Data.Repositories
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        private DataContext _context;

        public EntityBaseRepository(DataContext context)
        {
            _context = context;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<int> Count()
        {
            return await _context.Set<T>().CountAsync();
        }
        public virtual async Task<IEnumerable<T>> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.ToListAsync();
        }

        public T GetSingle(long id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        }
        public async Task<T> GetSingleAsync(long id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.Where(predicate).FirstOrDefaultAsync();
        }
        public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate).FirstOrDefault();
        }
        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }
        public virtual async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual IQueryable<T> FindQueryBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public virtual async Task<IEnumerable<T>> FindByAsyncIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.Where(predicate).ToListAsync();
        }

        public virtual void Add(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);
        }

        public virtual void Update(T entity)
        {
            entity.ModifiedDate = DateTime.Now;
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            var cols = dbEntityEntry.Collections;
            foreach (var colection in cols)
            {
                var subentity = colection.CurrentValue;
                foreach (var item in subentity)
                {
                    if ((item as EntityBase).Id == 0)
                        _context.Entry(item).State = EntityState.Added;
                    else if ((item as EntityBase).Delete)
                        _context.Entry(item).State = EntityState.Deleted;
                    else
                        _context.Entry(item).State = EntityState.Modified;

                }
            }
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Update(T entity, string excludeProperties = "")
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            var cols = dbEntityEntry.Collections;
            foreach (var colection in cols)
            {
                var subentity = colection.CurrentValue;
                foreach (var item in subentity)
                {
                    if ((item as EntityBase).Id == 0)
                        _context.Entry(item).State = EntityState.Added;
                    else if ((item as EntityBase).Delete)
                        _context.Entry(item).State = EntityState.Deleted;
                    else
                        _context.Entry(item).State = EntityState.Modified;
                }
            }

            dbEntityEntry.State = EntityState.Modified;
            dbEntityEntry.Property("CreatedDate").IsModified = false;
            foreach (var excludeProp in excludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                dbEntityEntry.Property(excludeProp).IsModified = false;
            }
        }

        public virtual void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);

            foreach (var entity in entities)
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        public virtual async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}
