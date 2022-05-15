using System.Linq;
using System.Web.Mvc;
using QLKS.WebApp.DAL;

namespace QLKS.WebApp.Areas.Adm.Controllers
{
    public class DashboardController : AdminController
    {
        private HotelDbContext db = new HotelDbContext();
        // GET: Adm/Dashboard
        public ActionResult Index()
        {
            ViewBag.numUsers = db.IdentityUsers.Count();
            ViewBag.numHotels = db.Suppliers.Count();
            return View();
        }
    }
}