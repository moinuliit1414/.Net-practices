using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DAL = JetEngine.DataAccessLayer;
using Repo = JetEngine.Repository;


namespace JetEngine.Repository.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine("START.....");
            Repo.UnitOfWork unitOfWork = new Repo.UnitOfWork(new DAL.Model.JetEngineContext());

            DAL.Model.Policy policy = new DAL.Model.Policy();
            policy.PolicyId = new Guid();
            //policy.IsActive = true;
            //policy.PolicyKey = "PASS_EXPIRE_DAYS";
            //policy.PolicyName = "PASS_EXPIRE_DAYS";
            //policy.PolicyValue = "90";
            //policy.PolicyValueXml = "";
            //policy.StartDate = new DateTime();
            //policy.EndDate = new DateTime();

            unitOfWork.Repository<Repo.Implement.PolicyRepository>().Get(policy.PolicyId);
            unitOfWork.Commit();

        }
    }
}
