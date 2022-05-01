using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using QLKS.WebApp.DAL;
using QLKS.WebApp.Models;

namespace QLKS.WebApp.Areas.Adm.Controllers
{
    public class CategoryController : AdminController
    {
        private HotelDbContext db = new HotelDbContext();


        // GET: Adm/Category
        public ActionResult Index()
        {
            var categories = db.Categories
                .OrderBy(c => c.ParentId)
                .ThenBy(x => x.OrderNo);

            return View(categories.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override bool OnUpdateToggle(string propName, bool value, object[] keys)
        {
            string query = string.Format(
                "UPDATE db0.Categories SET {0} = @p0 WHERE CategoryId = @p1", propName);
            return db.Database.ExecuteSqlCommand(query, value, keys[0]) > 0;
        }

        /// <summary>
        /// Phương thức lấy danh sách tất cả các nhóm mặt hàng
        /// và gom nhóm chúng theo thứ tự phân cấp để hiển thị trên
        /// cây hoặc menu,...
        /// </summary>
        /// <returns>
        /// Trả về các nhóm mặt hàng sau khi đã gom nhóm
        /// theo thứ tự phân cấp nhóm cha - nhóm con.
        /// </returns>

        private List<Category> PopulateCategories()
        {
            var allCates = db.Categories
                .OrderBy(x => x.ParentId)
                .ThenBy(x => x.OrderNo)
                .ToList();

            // Chỉ lấy những nhóm mặt hàng cha của tất cả nhóm khác
            var gruopedCates = allCates
                .Where(x => !x.ParentId.HasValue || x.ParentLevel == 0)
                .ToList();

            // Với mỗi nhóm cha, tìm các nhóm con của nó
            foreach (var category in gruopedCates)
            {
                AddSubCategory(category, allCates);
            }

            return gruopedCates;
        }


        /// <summary>
        /// Phương thức lấy các nhóm con của nhóm category
        /// từ danh sách tất cả các nhóm allCates
        /// </summary>
        /// <param name="category">Nhóm mặt hàng cha cần tìm nhóm con</param>
        /// <param name="allCates">Danh sách tất cả các nhóm mặt hàng</param>
        private void AddSubCategory(Category category, List<Category> allCates)
        {
            // Tìm các nhóm con của nhóm category
            category.ChildCates = allCates
                .Where(x => x.ParentId == category.CategoryId)
                .ToList();

            // Với nhóm con, gọi đệ quy để tìm nhóm con của chúng
            foreach (var subCate in category.ChildCates)
            {
                AddSubCategory(subCate, allCates);
            }
        }

        // GET: Adm/Category
        public ActionResult List()
        {
            var categories = PopulateCategories();
            return View(categories);
        }

        /// <summary>
        /// Phương thức cập nhật lại thứ tự các nhóm mặt hàng
        /// </summary>
        /// <param name="cid">Mã nhóm mặt hàng đổi thứ tự</param>
        /// <param name="pid">Mã nhóm mặt hàng cha</param>
        /// <param name="sidlings">Mã các nhóm mặt hàng anh em</param>
        /// <returns>Trả về true nếu cập nhật thành công</returns> 

        [HttpPost]
        public JsonResult Roerder(Guid cid, Guid pid, int[] siblings)
        {
            var success = true;
            try
            {
                StringBuilder query = new StringBuilder();

                // Tạo các chuỗi truy vấn cập nhật thuộc tính OrderNo
                for (int i = 0; i < siblings.Length; i++)
                {
                    query.AppendFormat("UPDATE dbo.Categories SET OrderNo = {0}" +
                                       "WHERE CategoryId = {1};", i + 1, siblings[i]);
                    query.AppendLine();
                }

                // Tạo chuỗi truy vấn cập nhật thuộc tính ParentId
                if (pid != null)
                {
                    query.AppendFormat("UPDATE dbo.Categories SET ParentId = {0}" +
                                       "WHERE CategoryId = {1};", pid, cid);
                }
                else
                {
                    query.AppendFormat("UPDATE dbo.Categories SET ParentId = null" +
                                       "WHERE CategoryId = {0};", cid);
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return Json(success);
        }

        /// <summary>
        /// Phương thức chuyển một danh sách mặt hàng (đã được gom
        /// nhóm theo kiểu phân cấp cha - con) thành một danh sách nhóm
        /// mặt hàng mới (không phân cấp) để hiển thị lên dropdownlist.
        /// </summary>
        /// <param name="source">Nhóm mặt hàng có cấu trúc phân cấp</param>
        /// <param name="disableIds">Mảng Id của những nhóm không được chọn</param>
        /// <param name="level">Mức của nút con trong cây phân cấp</param>
        /// <param name="result">Danh sách nhóm mặt hàng mới</param>
        private void ConvertToTreeStructure(IEnumerable<Category> source, List<Guid> disableIds, int level,
            List<Category> result)
        {
            // Duyệt qua từng nhóm mặt hàng trong nút cha
            foreach (var item in source)
            {
                var cateName = (level > 0) ? " ".PadLeft(level + 1, '-') : "";
                cateName += item.Name;

                // Tạo nhóm mặt hàng mới, thêm vào kết quả
                result.Add(new Category
                {
                    CategoryId = item.CategoryId,
                    Name = cateName
                });

                // Chỉ cho người dùng chọn nhóm cha là mức 0, 1
                if (level > 1)
                {
                    disableIds.Add(item.CategoryId);
                }

                ConvertToTreeStructure(item.ChildCates, disableIds, level + 1, result);
            }
        }

        /// <summary>
        /// Phương thức kiểm tra tính hợp lệ của tập tin được upload
        /// </summary>
        /// <param name="category">Nhóm mặt hàng đang được cập nhật</param>
        private void InitFormData(Category category)
        {
            // Lấy tất cả các nhóm mặt hàng và gom nhóm chúng
            var gruopedCates = PopulateCategories();

            // Tạo hai danh sách kết quả
            var disableIds = new List<Guid>();
            var categories = new List<Category>();

            // Chuyển danh sách nhóm mặt hàng đã gom nhóm
            // về lại dạng thông thường để hiện thị lên DropDownList
            ConvertToTreeStructure(gruopedCates, disableIds, 0, categories);

            if (category.ParentLevel > 0)
            {
                ViewBag.ParentLevel = new SelectList(categories, "CategoryId", "name", (object)null, disableIds);
            }
        }

        /// <summary>
        /// Phương thức kiểm tra tính hợp lệ của tập tin được upload
        /// </summary>
        /// <param name="upload">Tập tin đang được upload</param>
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

        /// <summary>
        /// Phương thức lưu nội dung tập tin xuống thư mục Upload/Icons
        /// và lấy thông tin đường dẫn gán cho thuộc tính IconPath của category
        /// </summary>
        /// <param name="upload">Tập tin đang được upload và cần được lưu</param>
        /// <param name="category">Nhóm mặt hàng được thêm mới thêm mới hoặc cập nhật</param>
        private void SaveUpLoadFile(HttpPostedFileBase upload, Category category)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                // Xóa Icon cũ nếu có
                if (!string.IsNullOrEmpty(category.IconPath))
                {
                    var oldFilePath = Server.MapPath(category.IconPath);
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
                category.IconPath = Path.Combine("~/Uploads/Icons", uniqueFileName);
            }
        }


        public ActionResult Create()
        {
            var emptyCate = new Category();
            InitFormData(emptyCate);
            return View(emptyCate);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase upload,
            [Bind(Include = "Name, Alias, Size, Description, Actived, ParentLevel, OrderNo")]
            Category category)
        {
            ValiUploadImage(upload);

            if (db.Categories.SingleOrDefault(x => x.Alias == category.Alias) != null)
            {
                ModelState.AddModelError("Alias", "Alias này đã tồn tại trong CSDL");
            }

            if (ModelState.IsValid)
            {
                // Lưu hình được upload lên server
                SaveUpLoadFile(upload, category);
                category.CategoryId = Guid.NewGuid();
                //Thêm nhóm mặt hàng vào csdl
                db.Categories.Add(category);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            InitFormData(category);
            return View(category);
        }
        // Get: Adm/category/Edit/guid
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            InitFormData(category);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase upload,
            [Bind(Include = "CategoryId, Name, Alias, Size, Description, IconPath , Actived, ParentId ,ParentLevel, OrderNo, RowVersion")]
            Category category)
        {
            ValiUploadImage(upload);

            if (db.Categories.SingleOrDefault(x => x.Alias == category.Alias && x.CategoryId != category.CategoryId) != null)
            {
                ModelState.AddModelError("Alias", "Alias này đã tồn tại trong CSDL");
            }

            if (ModelState.IsValid)
            {
                // Lưu hình được upload lên server
                SaveUpLoadFile(upload, category);
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            InitFormData(category);
            return View(category);
        }
    }
}