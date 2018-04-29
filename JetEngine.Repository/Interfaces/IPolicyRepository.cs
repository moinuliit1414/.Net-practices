using System;
using System.Collections.Generic;
using System.Text;
using JetEngine.DataAccessLayer.Model;

namespace JetEngine.Repository.Interfaces
{
    public interface IPolicyRepository : IRepository<Policy>
    {
        IEnumerable<Policy> GetActivePolicy();
    }
}
