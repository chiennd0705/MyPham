﻿using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class Default1Controller : Controller
    {
        //
        // GET: /Manage/Default1/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Manage/Default1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Manage/Default1/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Manage/Default1/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manage/Default1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Manage/Default1/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manage/Default1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Manage/Default1/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}