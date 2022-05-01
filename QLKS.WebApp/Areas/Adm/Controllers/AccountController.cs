using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PagedList;
using QLKS.WebApp.DAL;
using QLKS.WebApp.Models;

namespace QLKS.WebApp.Areas.Adm.Controllers
{
    public class AccountController : Controller
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection upload)
        {
            var accounts = new Account();
            try
            {
                UpdateModel(accounts, new[]
                {
                    "Profile.AccountID", "UserName", "Email", "Profile.Password", "PhoneNumber",
                });
                if (db.IdentityUsers.SingleOrDefault(x => x.UserName == accounts.Email && x.Profile.AccountID != accounts.Profile.AccountID) != null)
                {
                    ModelState.AddModelError("UserName", "UserName này đã tồn tại!!!");
                }
                if (ModelState.IsValid)
                {
                    accounts.Profile.AccountID = Guid.NewGuid().ToString();
                    db.Users.Add(accounts);
                    db.SaveChanges();
                    return Redirect(accounts.Profile.AccountID);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View(accounts);
        }
    }
}