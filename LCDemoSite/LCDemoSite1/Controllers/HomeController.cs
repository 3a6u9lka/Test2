using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using LCDemoSite1.Models;
using Servise.DataProviders;

namespace LCDemoSite1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string action)
        {
            if (action == "getOne")
            {
                CreatPeople();
            }
            else if (action == "getReport")
            {
                //setRespons();
            }
            return View();
        }

        private void CreatPeople()
        {
            using (var client = new HttpClient())
            {
                var result = client.GetAsync("http://localhost:49686/api/").Result;
                if (result.IsSuccessStatusCode)
                {

                }
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}