using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VerteBienV1.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}
        [Authorize(Roles = "administrador")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Membresias()
        {


            return View();
        }
        [Authorize(Roles = "administrador")]
        public ActionResult Pruebapagos()
        {


            return View();
        }
        [Authorize(Roles = "administrador")]
        public ActionResult pruebaPago()
        {


            return View();
        }
        public ActionResult Error()
        {


            return View();
        }
    }
}