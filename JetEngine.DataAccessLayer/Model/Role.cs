using System;
using System.Collections.Generic;

namespace JetEngine.DataAccessLayer.Model
{
    public partial class Role
    {
        public Role()
        {
            UserRole = new HashSet<UserRole>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserRole> UserRole { get; set; }
    }
}
