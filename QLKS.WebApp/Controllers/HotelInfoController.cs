using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QLKS.WebApp.DAL;
using QLKS.WebApp.Models;

namespace QLKS.WebApp.Controllers
{
    public class HotelInfoController : Controller
    {
        HotelDbContext db = new HotelDbContext();
        // GET: HotelInfo
        public ActionResult Index(string keyword, int? page, int? pageSize)
        {
            var suppliers = db.Suppliers.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                suppliers = suppliers.Where(x => x.Name.Contains(keyword) ||
                                                 x.ContactName.Contains(keyword) ||
                                                 x.Address.Contains(keyword) ||
                                                 x.Description.Contains(keyword));
            }

            if (!page.HasValue || page.Value < 1) page = 1;
            if (!pageSize.HasValue || pageSize.Value < 5) pageSize = 5;

            ViewBag.Keyword = keyword;
            ViewBag.PageSize = new SelectList(new[] { 5, 10, 25, 50, 100 }, pageSize);
            ViewBag.CurrentPageSize = pageSize;

            var data = suppliers.OrderBy(x => x.Name).ToPagedList(page.Value, pageSize.Value);

            return View(data);
        }

        // GET: HotelInfo//Blog/alias
        public ActionResult Blog(string alias)
        {
            if (alias == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Supplier supplier = db.Suppliers.Find(id);
            var suppliers = db.Suppliers.AsQueryable();
            suppliers = suppliers.Where(x => x.Alias.Contains(alias));
            Supplier supplier = new Supplier();
            foreach (var item in suppliers)
            {
                supplier = item;
                break;
            }

            if (supplier == null)
            {
                return HttpNotFound();
            }

            return View(supplier);
        }

    }
}