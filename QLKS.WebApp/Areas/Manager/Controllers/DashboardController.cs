using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLKS.WebApp.Areas.Adm.Controllers;
using QLKS.WebApp.Controllers;

namespace QLKS.WebApp.Areas.Manager.Controllers
{
    public class DashboardController : ManagerController
    {
        // GET: Manager/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}