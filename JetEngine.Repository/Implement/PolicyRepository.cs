using JetEngine.DataAccessLayer.Model;
using JetEngine.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using JetEngine.DataAccessLayer.Model;

namespace JetEngine.Repository.Implement
{
    public class PolicyRepository : Repository<Policy>, IPolicyRepository
    {
        public PolicyRepository(JetEngineContext context):base(context)
        {
            //this._context = context;
            //_entities = context.Set<T>();
        }

        public IEnumerable<Policy> GetActivePolicy()
        {
            throw new NotImplementedException();
        }
    }
}
