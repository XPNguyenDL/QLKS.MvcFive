﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKS.WebApp.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult Index(string hotelName)
        {
            ViewBag.Name = hotelName;
            return View();
        }
    }
}