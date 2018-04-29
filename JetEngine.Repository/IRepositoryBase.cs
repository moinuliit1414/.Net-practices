using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetEngine.Repository
{
    public interface IRepositoryBase<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
