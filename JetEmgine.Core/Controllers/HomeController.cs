using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JetEmgine.Core.Models;
using DAL = JetEngine.DataAccessLayer;
using JetEngine.Repository.Interfaces;
using JetEngine.Repository.Implement;
using JetEngine.Repository;

namespace JetEmgine.Core.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            UnitOfWork unitOfWork = new UnitOfWork(new DAL.Model.JetEngineContext());

            DAL.Model.Policy policy = new DAL.Model.Policy();
            policy.PolicyId = new Guid();




            //Dictionary<String,>
            //policy =unitOfWork.Repository<DAL.Model.Policy>().Get(policy.PolicyId);
            policy.PolicyKey = "PASS_EXPIRE_DAYS";
            policy.PolicyName = "PASS_EXPIRE_DAYS";
            //policy = unitOfWork.Repository<DAL.Model.Policy>().Get(policy.PolicyId);
            //unitOfWork.Repository<DAL.Model.Policy>().Delete(policy);
            //unitOfWork.Commit();

            policy.IsActive = true;
            
            
            policy.PolicyValue = "90";
            policy.PolicyValueXml = "";
            policy.StartDate = DateTime.Now;
            policy.EndDate = DateTime.Now;

            //unitOfWork.Repository<DAL.Model.Policy>().Delete(policy);
            IPolicyRepository policyRepository = unitOfWork.Repository<PolicyRepository>();
            unitOfWork.Commit();
            //unitOfWork.Repository<Repo.PolicyRepository>().Insert(policy);

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
