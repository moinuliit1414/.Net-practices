using System;
using System.Collections.Generic;
using System.Text;
using DAL=JetEngine.DataAccessLayer.Model;
using JetEngine.Repository.Interfaces;
using JetEngine.Repository.Implement;
using JetEngine.Repository;
using System.Linq;
using JetEngine.Helper;

namespace JetEngine.Businesslayer.Policy
{
    public class PolicyBL
    {
        private Dictionary<String,DAL.Policy> _policies;
        private DateTime _policyBLCrearttime = DateTime.Now;
        private readonly bool _refreshPolicies;
        private readonly long _refreshInMiliseconds;
        private readonly IPolicyRepository _policyRepository;
        private readonly IPolicyMethods _policyMethods = new PolicyMethods();

        public PolicyBL(IPolicyRepository policyRepository,bool refreshPolicies=false, long refreshInMiliseconds=60000)
        {
            this._refreshPolicies = refreshPolicies;
            this._refreshInMiliseconds = refreshInMiliseconds;
            this._policyRepository = policyRepository;
            this.LoadPolices();
        }

        public PolicyBL(List<DAL.Policy> policies)
        {
            this._policies = (policies == null || !policies.Any()) ? new Dictionary<String,DAL.Policy>() : policies.ToDictionary(x => x.PolicyKey, x => x);
            this._refreshPolicies = false;
            
        }

        private void LoadPolices()
        {
            
            if (_policies==null || (this._refreshPolicies && Converter.DateTimAbseDiffInMiliseconds(_policyBLCrearttime,DateTime.Now)>_refreshInMiliseconds)) {
                var policies = _policyRepository.Where(p => p.IsActive == true && p.StartDate>=DateTime.Now && (!p.EndDate.HasValue ||(p.EndDate.HasValue && p.EndDate<=DateTime.Now))).ToList();

                this._policyBLCrearttime = DateTime.Now;
                this._policies = (policies == null || !policies.Any()) ? new Dictionary<String, DAL.Policy>() : policies.ToDictionary(x => x.PolicyKey, x => x);
                //this._policies = (policies == null|| !policies.Any()) ? new List<DAL.Policy>() : policies;
            }

        }

        public bool validateLoginPolicies() {
            //var passwordExpiryDays = _policies.Where(p => p.PolicyKey == "PASS_EXP_DAYES").OrderByDescending(p => p.StartDate).SingleOrDefault();
            if (_policies.ContainsKey("PASS_EXP_DAYES") && _policyMethods.IsPasswordExpired(DateTime.Now,_policies["PASS_EXP_DAYES"].PolicyValue)) {
                
                var exp = new Exception("",new Exception());
                //exp.
                throw new Exception("JetEngine.Businesslayer.Policy.PolicyBL.PASSWORD_EXPAIRED");
            }
            

            return true;
        }

        //public int GetPasswordExpiryDays()
        //{
        //    var days=_policies.Where(p => p.PolicyKey == "PASS_EXP_DAYES").OrderByDescending(p=>p.StartDate).SingleOrDefault();
        //    if (_policies.Any()) {
        //        try
        //        {
        //            return Int32.Parse(days.PolicyValue);
        //        }
        //        catch (Exception ex) {
        //            throw new Exception("Policy Value Parse error in GetPasswordExpiryDays() Method,Policy Key PASS_EXP_DAYES");
        //        }
                
        //    }

        //    return 0;
        //}

        public int GetPasswordMaxLength()
        {
            throw new NotImplementedException();
        }

        public int GetPasswordMinLength()
        {
            throw new NotImplementedException();
        }

        public string GetPasswordStrength()
        {
            throw new NotImplementedException();
        }

        public string GetPasswordStrength(string password)
        {
            throw new NotImplementedException();
        }

        public bool IsComplexPasswordRequired()
        {
            throw new NotImplementedException();
        }
    }
}
