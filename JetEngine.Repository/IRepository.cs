using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace JetEngine.Repository
{
    public interface IRepository<T> where T:class
    {
        T Get(object id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Where(Expression<Func<T,bool>> predicate);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
