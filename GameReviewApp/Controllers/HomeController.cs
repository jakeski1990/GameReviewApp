﻿using GameReviewApp.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameReviewApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeOrRedirectAttribute(Roles = "Admin,Moderator")]
        public ActionResult Admin()
        {
            return View();
        }
    }
}