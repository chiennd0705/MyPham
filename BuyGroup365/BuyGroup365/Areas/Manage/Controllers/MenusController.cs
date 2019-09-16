using Business.ClassBusiness;
using BuyGroup365.Areas.Manage.Models;
using Common;
using Common.util;
using MvcPaging;
using System;
using System.ServiceModel;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class MenusController : BaseController
    {
        //
        // GET: /Manage/CategoryProduct/

        #region Khai bao biến

        private MenusBusiness menusBusiness = new MenusBusiness();
        private readonly LoadCombo _loadCombo = new LoadCombo();

        public MenusController()
        {
        }

        #endregion Khai bao biến

        #region Category

        public ActionResult Index(string key, string TypeMenuSeach, int? page)
        {
            try
            {
                ViewData["status"] = true;
                if (string.IsNullOrEmpty(key))
                    key = string.Empty;
                if (string.IsNullOrEmpty(TypeMenuSeach))
                    TypeMenuSeach = "-1";
                ViewData["key"] = key;
                int currentPageIndex = page.HasValue ? page.Value : 1;
                var list = menusBusiness.GetList(key, long.Parse(TypeMenuSeach)).ToPagedList(currentPageIndex, 20);

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
        public ActionResult Update(long id, string btnSubmit, string MenuName, string Link, int TypeMenu, int Status, int Order, int ParentId, string BackGround, Boolean IsBackGround, int NewGroupID)
        {
            try
            {
                if (btnSubmit == "Thêm")
                {
                    //List<LoadCombo.DropdowMenus> listDropdowMenus = new List<LoadCombo.DropdowMenus>();
                    //ViewBag.ParentId = _loadCombo.SearchMenusByName(ref listDropdowMenus);
                    Menus menus = new Menus();
                    menus.MenuName = MenuName;
                    menus.Link = Link;
                    menus.Order = Order;
                    menus.Status = Status;
                    menus.TypeMenu = TypeMenu;
                    menus.CreateDate = DateTime.Now;
                    menus.ParentId = ParentId;
                    menus.IsBackGround = IsBackGround;
                    menus.NewGroupID = NewGroupID;
                    if (BackGround != "")
                    {
                        menus.BackGround = BackGround;
                    }

                    menusBusiness.AddNew(menus);
                }
                else
                {
                    //List<LoadCombo.DropdowMenus> listDropdowMenus = new List<LoadCombo.DropdowMenus>();
                    //ViewBag.ParentId = _loadCombo.SearchMenusByName(ref listDropdowMenus);

                    //ListGroup from name of group
                    //    ViewBag.listGroupNewsGroupWithNamme = _loadCombo.InitSelectListItemStatusNewsGroup();
                    var menus = menusBusiness.GetById(id);
                    menus.MenuName = MenuName;
                    menus.Link = Link;
                    menus.Order = Order;
                    menus.Status = Status;
                    menus.TypeMenu = TypeMenu;
                    menus.CreateDate = DateTime.Now;
                    menus.ParentId = ParentId;
                    menus.IsBackGround = IsBackGround;
                    menus.NewGroupID = NewGroupID;
                    if (BackGround != "")
                    {
                        menus.BackGround = BackGround;
                    }

                    menusBusiness.Edit(menus);
                }
                Response.Redirect("/Manage/Menus/Index");
            }
            catch (Exception ex)
            {
                Response.Redirect("/Manage/Menus/Index");
            }
            return View();
        }

        public ActionResult Delete(long id)
        {
            try
            {
                if (id > 0)
                {
                    //Thay đổi trạng thái danh mục
                    menusBusiness.Remove(id);
                    //status: 1(active), 0(private)
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                //write log
                throw;
            }
        }

        #endregion Category
    }
}