using System;
using System.Collections.Generic;

namespace JetEngine.DataAccessLayer.Model
{
    public partial class User
    {
        public User()
        {
            UserClaim = new HashSet<UserClaim>();
            UserLogin = new HashSet<UserLogin>();
            UserRole = new HashSet<UserRole>();
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }

        public ICollection<UserClaim> UserClaim { get; set; }
        public ICollection<UserLogin> UserLogin { get; set; }
        public ICollection<UserRole> UserRole { get; set; }
    }
}
