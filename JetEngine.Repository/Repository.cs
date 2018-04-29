using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace JetEngine.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected DbSet<T> _entities;

        public Repository(DbContext context) {
            this._context = context;
            this._entities = context.Set<T>();
        }

        public virtual void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public virtual IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public virtual T Get(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public virtual void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
    }
}
