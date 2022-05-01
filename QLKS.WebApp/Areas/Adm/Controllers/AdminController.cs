using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKS.WebApp.Areas.Adm.Controllers
{
    
    [Authorize(Roles = "Admin,Manager,Salesman")]
    public class AdminController : Controller
    {
        /// <summary>
        /// Phương thức chuyển hướng người dùng dựa vào
        /// hành động mà người dùng chọn trong form
        /// cập nhật hay thêm mới dữ liệu
        /// </summary>
        /// <returns>
        /// Chuyển hướng người dùng tới trang Create
        /// Nếu người dùng chọn nút lưu & thêm mới
        /// Nếu người dùng chọn nút lưu & đóng chuyển tới trang Index
        /// Nếu chọn nút lưu & cập nhật hoặc
        /// Tạm lưu thì chuyển tới trang Edit</returns>
        protected virtual ActionResult Redirect(Object id)
        {
            var saveAction = Request.Form["save-action"];
            switch (saveAction)
            {
                case "save-new":
                    return RedirectToAction("Create");
                case "save-edit":
                    return RedirectToAction("Edit", new { id });
                default:
                    return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Phương thức thực hiện việc thay đổi giá trị của một
        /// thuộc tính có kiểu true/false (Kiểu bit trong Csdl)
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnUpdateToggle(string proName, bool value, object[] keys)
        {
            return true;
        }
        
        /// <summary>
        /// </summary>
        /// <param name="args">
        /// chuổi chứa tên thuộc tính, giá trĩ hiện tại và Id
        /// của mâu tin cần cập nhật phân tách nhau bởi dấu _
        /// </param>
        /// <returns>
        /// Trả về kiểu đối tượng Json cho biết có cập nhật thành công hay không.
        /// Nếu có, dữ liệu đi kèm sẽ là chuỗi args mới
        /// chứ giá trị sau khi cập nhật. Nếu không, dữ liệu đi kèm sẽ thông báo lỗi</returns>
        [HttpPost]
        public JsonResult UpdateToggle(string args)
        {
            bool success = false;
            string html = String.Empty;

            try
            {

                var data = args.Split('_');
                var propName = data[0]; // Tên thuộc tính
                var value = !bool.Parse(data[1]); // Giá trị hiện tại
                var keys = data.Skip(2).ToArray(); // ID Mẫu tin

                //Gọi hàm cập nhật giá trị thuộc tính
                if (OnUpdateToggle(propName, value, keys))
                {
                    success = true;
                    // Tạo chuỗi tương tự args với giá trị mới
                    html = string.Format("{0}_{1}_{2}",
                        propName, value.ToString().ToLower(),
                            string.Join("_", keys));


                }
            }
            catch (Exception ex)
            {
                success = false;
                html = ex.Message;
            }

            return Json(new
            {
                Result = success,
                Message = html
            });
        }
    }
}