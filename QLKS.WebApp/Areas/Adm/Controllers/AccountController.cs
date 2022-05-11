using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using PagedList;
using QLKS.WebApp.DAL;
using QLKS.WebApp.Models;

namespace QLKS.WebApp.Areas.Adm.Controllers
{
    public class AccountController : AdminController
    {
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private HotelDbContext db = new HotelDbContext();

        private ApplicationUserManager _userManager;

        // GET: Adm/Account
        public ActionResult Index()
        {
            var accounts = db.IdentityUsers.ToList();

            return View(accounts);
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home", new { area = "" });
        }


        // GET: Adm/Account/Create
        public ActionResult Create()
        {
            var roles = db.Roles.ToList();
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection data, QLKS.WebApp.DAL.HotelDbContext context, HttpPostedFileBase upload)
        {
            var accounts = new Account();
            var userManager = new UserManager<Account>(new UserStore<Account>(context));
            var roles = db.Roles.ToList();
            ViewBag.Roles = roles;
            try
            {
                UpdateModel(accounts, new[]
                {
                    "UserName", "Email", "PhoneNumber", "Profile", "RoleTemps"
                });
                if (db.IdentityUsers.SingleOrDefault(x => x.UserName == accounts.UserName && x.Id != accounts.Id) != null)
                {
                    ModelState.AddModelError("UserName", "UserName này đã tồn tại!!!");
                }
                if (ModelState.IsValid)
                {

                    accounts.Profile.AccountID = Guid.NewGuid().ToString();

                    SaveUpLoadFile(upload, accounts);

                    var result = userManager.Create(accounts, accounts.Profile.Password);

                    if (result.Succeeded)
                    {
                        foreach (var nameTemp in accounts.RoleTemps)
                        {
                            userManager.AddToRole(accounts.Id, nameTemp);
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(accounts);
        }

        private void SaveUpLoadFile(HttpPostedFileBase upload, Account account)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                // Xóa Icon cũ nếu có
                if (!string.IsNullOrEmpty(account.Profile.Picture))
                {
                    var oldFilePath = Server.MapPath(account.Profile.Picture);
                    System.IO.File.Delete(oldFilePath);
                }

                // Lấy đường dẫn tuyệt đối của thư mục lưu file
                var tagerFolder = Server.MapPath("~/Uploads/Pictures");

                // Tạo thêm mới cho tập tin và đường dẫn tuyệt đối để lưu
                var uniqueFileName = DateTime.Now.Ticks + "_" + upload.FileName;
                var targetFilePath = Path.Combine(tagerFolder, uniqueFileName);

                // Lưu File
                upload.SaveAs(targetFilePath);

                // Lấy đường dẫn tương đối của tập tin vùa upload
                account.Profile.Picture = Path.Combine("~/Uploads/Pictures", uniqueFileName);
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


        // Post: adm/account/Edit/5
        public ActionResult Edit(string id)

        {
            var accounts = new Account();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            accounts = db.IdentityUsers.Find(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }

            var roleId = accounts.Roles.Select(s => s.RoleId).ToList();
            var roles = db.Roles.ToList();
            ViewBag.Roles = roles;
            var roleText = JsonConvert.SerializeObject(roleId);
            ViewBag.RoleId = roleText;
            // ViewBag.RoleId = roleId;
            return View(accounts);
        }



        // Post: adm/account/edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HttpPostedFileBase upload,
            [Bind(Include = "Id, UserName, Email, PhoneNumber, Profile, RoleTemps")]
            Account account, HotelDbContext context, string id)
        {
            var userManager = new UserManager<Account>(new UserStore<Account>(context));
            var roles = db.Roles.ToList();
            ViewBag.Roles = roles;



            var tempAccount = db.IdentityUsers.Find(id);
            try
            {

                if (!ModelState.IsValid)
                {
                    //  DeleteRoles(temp, tempAccount);
                    SaveUpLoadFile(upload, account);
                    
                    //userManager.Update()

                    db.SaveChanges();
                    var oldRole = db.UserInRoles.Where(s => s.UserId == id).ToList();
                    foreach (var role in oldRole)
                    {
                        db.Entry(role).State = EntityState.Deleted;
                    }
                    db.SaveChanges();
                    foreach (var item in account.RoleTemps)
                    {
                        db.UserInRoles.Add(new IdentityUserRole()
                        {
                            RoleId = item,
                            UserId = id
                        });
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(account);
        }

        private void DeleteRoles(List<IdentityUserRole> userRoles, Account temp)
        {
            for (int i = 0; i < userRoles.Count; i++)
            {
                temp.Roles.Remove(userRoles[i]);
            }

            db.SaveChanges();
        }
    }
}