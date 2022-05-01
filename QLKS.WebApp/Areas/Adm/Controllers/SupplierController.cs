using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QLKS.WebApp.DAL;
using QLKS.WebApp.Models;

namespace QLKS.WebApp.Areas.Adm.Controllers
{
    
    public class SupplierController : AdminController
    {
        private HotelDbContext db = new HotelDbContext();
        // GET: Adm/Supplier
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
            ViewBag.PageSize = new SelectList(new[] {5, 10, 25, 50, 100}, pageSize);
            ViewBag.CurrentPageSize = pageSize;

            var data = suppliers.OrderBy(x => x.Name).ToPagedList(page.Value, pageSize.Value);
            return View(data);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }

        protected override bool OnUpdateToggle(string propName, bool value, object[] keys)
        {
            try
            {
                string query = string.Format(
                    "UPDATE dbo.Suppliers SET {0} = @p0 WHERE SupplierId = @p1", propName);
                return  db.Database.ExecuteSqlCommand(query, value, keys[0]) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // GET: Adm/Supplier/Create
        public ActionResult Create()
        {
            return View();
        }

        //Post: Adm/Supplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection data)
        {
            var supplier = new Supplier();
            try
            {
                UpdateModel(supplier, new[]
                {
                    "Name", "Alias", "Description", "ContactName",
                    "Address", "UserName", "Phone", "HomePage",
                    "Actived", "ContactTitle"
                });
                if (db.Suppliers.SingleOrDefault(x => x.Alias == supplier.Alias && x.SupplierId != supplier.SupplierId) != null)
                {
                    ModelState.AddModelError("Alias", "Alias này đã tồn tại trong CSDL");
                }
                if (ModelState.IsValid)
                {
                    supplier.SupplierId = Guid.NewGuid();
                    db.Suppliers.Add(supplier);
                    db.SaveChanges();
                    return Redirect(supplier.SupplierId);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View(supplier);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }

            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection data)
        {
            var supplier = new Supplier();
            try
            {

                UpdateModel(supplier, new[]
                {
                    "SupplierId", "Alias", "Name", "Description", "ContactName",
                    "Address", "UserName", "Phone", "HomePage",
                    "Actived", "ContactTitle", "RowVersion"
                });
                if (db.Suppliers.SingleOrDefault(x => x.Alias == supplier.Alias && x.SupplierId != supplier.SupplierId) != null)
                {
                    ModelState.AddModelError("Alias", "Alias này đã tồn tại trong CSDL");
                }
                if (ModelState.IsValid)
                {
                    db.Entry(supplier).State = EntityState.Modified;
                    db.SaveChanges();
                    return Redirect(supplier.SupplierId);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(supplier);
        }

        // Post: Adm/Supplier/Delete/guid

        
    }
}