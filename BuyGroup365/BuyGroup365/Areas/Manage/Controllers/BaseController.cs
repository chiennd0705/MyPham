﻿using Common.util;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext rc)
        {
            base.Initialize(rc);

            if (!SessionUtility.CheckLogin(Session))
            {
                if (rc.HttpContext.Request.Url != null)
                    Response.Redirect("/Manage/Login/Login?returnUrl=" + rc.HttpContext.Request.Url.PathAndQuery);
            }
            else
            {
                if (rc.HttpContext.Request.Url != null && !SessionUtility.CheckPermission(Session, rc.HttpContext.Request.Url.PathAndQuery))
                {
                    Response.Redirect("/Manage/Error/NotPermissionAdmin");
                }
            }
        }

        //
        // GET: /Manage/Base/
    }
}