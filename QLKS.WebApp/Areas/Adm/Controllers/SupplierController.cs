using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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

        private void ValiUploadImage(HttpPostedFileBase upload)
        {
            var allowImageFileTypes = new[] { ".jpg", ".jpeg", ".gif", ".png" };

            // Kiểm tra tính hợp lệ của tập tin được upload
            if (upload != null)
            {
                // Kiểm tra trường hợp file rỗng
                if (upload.ContentLength == 0)
                {
                    ModelState.AddModelError("IconPath", "Tập tin không có nội dung");
                }

                // Kiểm tra trường hợp file quá lớn
                if (upload.ContentLength > 1 * 1024 * 1024)
                {
                    ModelState.AddModelError("IconPath", "Dung lượng file quá lớn (>1MB)");
                }

                // Lấy phần mở rộng của tên file
                var imageExt = Path.GetExtension(upload.FileName);

                // Kiểm tra trường hợp upload các file không đúng định dạng cho phép
                if (!allowImageFileTypes.Contains(imageExt))
                {
                    ModelState.AddModelError("IconPath",
                        "Chỉ được phép upload tập tin jpg, jpeg, gif và png");
                }
            }
        }

        private void SaveUpLoadFile(HttpPostedFileBase upload, Supplier supplier)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                // Xóa Icon cũ nếu có
                if (!string.IsNullOrEmpty(supplier.Image))
                {
                    var oldFilePath = Server.MapPath(supplier.Image);
                    System.IO.File.Delete(oldFilePath);
                }

                // Lấy đường dẫn tuyệt đối của thư mục lưu file
                var tagerFolder = Server.MapPath("~/Uploads/Icons");

                // Tạo thêm mới cho tập tin và đường dẫn tuyệt đối để lưu
                var uniqueFileName = DateTime.Now.Ticks + "_" + upload.FileName;
                var targetFilePath = Path.Combine(tagerFolder, uniqueFileName);

                // Lưu File
                upload.SaveAs(targetFilePath);

                // Lấy đường dẫn tương đối của tập tin vùa upload
                supplier.Image = Path.Combine("~/Uploads/Icons", uniqueFileName);
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
        public ActionResult Create(FormCollection data, HttpPostedFileBase upload)
        {
            var supplier = new Supplier();
            try
            {
                ValiUploadImage(upload);
                UpdateModel(supplier, new[]
                {
                    "Name", "Alias", "Description", "ContactName",
                    "Address", "UserName", "Phone", "HomePage",
                    "Actived", "ContactTitle", "Image", "ShortInfo"
                });
                if (db.Suppliers.SingleOrDefault(x => x.Alias == supplier.Alias && x.SupplierId != supplier.SupplierId) != null)
                {
                    ModelState.AddModelError("Alias", "Alias này đã tồn tại trong CSDL");
                }
                if (ModelState.IsValid)
                {
                    supplier.SupplierId = Guid.NewGuid();
                    SaveUpLoadFile(upload, supplier);
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
        public ActionResult Edit(FormCollection data, HttpPostedFileBase upload)
        {
            var supplier = new Supplier();
            try
            {
                ValiUploadImage(upload);
                UpdateModel(supplier, new[]
                {
                    "SupplierId", "Alias", "Name", "Description", "ContactName",
                    "Address", "UserName", "Phone", "HomePage",
                    "Actived", "ContactTitle", "RowVersion", "Image", "ShortInfo"
                });
                if (db.Suppliers.SingleOrDefault(x => x.Alias == supplier.Alias && x.SupplierId != supplier.SupplierId) != null)
                {
                    ModelState.AddModelError("Alias", "Alias này đã tồn tại trong CSDL");
                }
                if (ModelState.IsValid)
                {
                    SaveUpLoadFile(upload, supplier);
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
        public JsonResult Delete(Guid id)
        {
            var success = true;
            try
            {
                var supplier = db.Suppliers.Find(id);
                db.Suppliers.Remove(supplier);
                db.SaveChanges();
            }
            catch (Exception)
            {
                success = false;
            }
            
            return Json(success);
        }

    }
}