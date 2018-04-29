using JetEngine.DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetEngine.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly JetEngineContext _context;
        protected DbSet<T> _entities;
        protected Type _type;
        protected string errorMessage = string.Empty;

        public RepositoryBase(JetEngineContext context)
        {
            this._context = (JetEngineContext)context;
            this._entities = context.Set<T>();
            this._type = typeof(T);
        }
        public IEnumerable<T> GetAll()
        {
            return _entities.AsEnumerable();
        }

        public T Get(object id) {
            return _entities.Find(id);
        }

        //public T Get(object id1, object id2)
        //{
        //    return _entities.Find(id1,id2);
        //}

        public T GetAll(params object[] keyvalue)
        {
            return (T)_context.Find(_entities.GetType(), keyvalue);
        }

        //public IEnumerable<T> GetAll(Dictionary<String,Object> Parms)
        //{
        //    //return _entities.Where(x=>x.GetType.ToString())
        //    return Parms.Where(y => _entities.Any(z => z.ToString() == y.Key));
        //}

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
            //_context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            //_context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Remove(entity);
            //_context.SaveChanges();
        }        
    }
}
