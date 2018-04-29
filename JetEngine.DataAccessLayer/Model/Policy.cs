using System;
using System.Collections.Generic;

namespace JetEngine.DataAccessLayer.Model
{
    public partial class Policy
    {
        public Guid PolicyId { get; set; }
        public string PolicyKey { get; set; }
        public string PolicyName { get; set; }
        public string PolicyValue { get; set; }
        public string PolicyValueXml { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
