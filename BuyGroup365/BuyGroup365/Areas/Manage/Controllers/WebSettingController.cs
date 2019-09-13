using Business.ClassBusiness;
using Common;
using Common.util;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class WebSettingController : BaseController
    {
        private SystemSettingBusiness _systemSettingBusiness = new SystemSettingBusiness();
        private FriendlyUrlBusiness friendlyUrlBussiness = new FriendlyUrlBusiness();
        private List<FriendlyUrl> ListFriendlyUrl = new List<FriendlyUrl>();

        //
        // GET: /Manage/WebSetting/
        public ActionResult Index(string key, int? page)
        {
            try
            {
                ViewData["status"] = true;
                ViewData["key"] = key;
                int currentPageIndex = page.HasValue ? page.Value : 1;

                var listobj = _systemSettingBusiness.GetDynamicQuery();
                //     var list = _catalogsBusiness.GetDynamicQuery().ToList().ToPagedList(currentPageIndex, 20);
                var list = _systemSettingBusiness.GetByKey(key).ToPagedList(currentPageIndex, 20);

                return View(list);
            }
            catch (FaultException ex)
            {
                var exep = Function.GetExeption(ex);
                var codeExp = exep[1];
                string url = "Error/ErrorFunction/" + codeExp;
                return RedirectToActionPermanent(url);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(string title, string key, string valuesystem, string des)
        {
            try
            {
                SystemSetting entity = new SystemSetting();
                entity.Key = key;
                entity.Value = valuesystem;
                entity.Title = title;
                entity.DesCription = des;
                _systemSettingBusiness.AddNew(entity);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //witrelog
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(long ID, string title, string key, string valuesystem, string des)
        {
            var tag = _systemSettingBusiness.GetById(ID);
            SystemSetting entity = new SystemSetting();

            entity.Key = key;
            entity.Value = valuesystem;
            entity.Title = title;
            entity.DesCription = des;

            entity.ModifyDate = DateTime.Now;
            _systemSettingBusiness.Edit(entity);
            return View();
        }
    }
}