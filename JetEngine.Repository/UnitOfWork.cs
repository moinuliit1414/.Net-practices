using JetEngine.DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JetEngine.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly JetEngineContext _context;
        private bool disposed;
        private Dictionary<string, object> repositories;

        public UnitOfWork(DbContext context)
        {
            this._context = (JetEngineContext)context;
        }

        //public UnitOfWork()
        //{
        //    _context = new JetEngineContext();
        //}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Commit()
        {
            _context.SaveChanges();
            //Dispose();
        }

       

        public void Rollback()
        {

            foreach (var entry in _context.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList())
                //.Where(e => e.State != null))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
                
                
            }
            //Dispose();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public RepositoryBase<T> RepositoryBase<T>() where T : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(RepositoryBase<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                repositories.Add(type, repositoryInstance);
            }
            return (RepositoryBase<T>) repositories[type];
        }

        public T Repository<T>() where T : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(T);
                var repositoryInstance = (T)Activator.CreateInstance(typeof(T), _context);
                repositories.Add(type, repositoryInstance);
            }
            return (T)repositories[type];
        }
    }
}
