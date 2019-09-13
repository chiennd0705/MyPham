using Business.ClassBusiness;
using BuyGroup365.Areas.Manage.Controllers;
using BuyGroup365.Models.Member;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Models
{
    public class LoadCombo
    {
        private readonly CatalogsBusiness _catalogsBusiness = new CatalogsBusiness();
        private readonly ManufacturersBusiness _manufacturersBusiness = new ManufacturersBusiness();
        private readonly Catalogs_ManufacturersBusiness _catalogsManufacturersBusiness = new Catalogs_ManufacturersBusiness();
        private readonly NewsBusiness _newsBusiness = new NewsBusiness();
        private readonly NewsGroupBusiness _newsGroupBusiness = new NewsGroupBusiness();
        private readonly CatalogDownloadsBusiness _CatalogDownloadBusiness = new CatalogDownloadsBusiness();
        private readonly MenusBusiness _menuBusiness = new MenusBusiness();

        public List<SelectListItem> SearchCategoryByName(ref List<LoadDropdown.DropdowCate> listDropdowCate)
        {
            var list = SearchCategoryByParentId("└-------", -1, ref listDropdowCate);
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "└Danh mục gốc", Value = "-1", Selected = true });
            foreach (var item in list)
            {
                listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return listItems;
        }

        public string SearchCategoryByNamecb(ref List<LoadDropdown.DropdowCate> listDropdowCate)
        {
            var list = SearchCategoryByParentId("└", -1, ref listDropdowCate);
            var listItems = new List<SelectListItem>();
            string htmlcate = "";
            foreach (var item in list)
            {
                htmlcate += "<label class=\"checkbox-inline\" style=\"width:83%\" ><input id=\"cb_cata" + item.Id + "\"  onclick=\"GetValueManuface(" + item.Id + ")\" value=\"" + item.Id + "\" type=\"checkbox\" name=\"catalogProducts\">" + item.Name + "</label><span id=\"catalogProducts" + item.Id + "\"></span>";
            }
            return htmlcate;
        }

        public string SearchCategoryByNamecb(ref List<LoadDropdown.DropdowCate> listDropdowCate, List<CatalogProducts> CatalogProducts, long? catalogmain)
        {
            var list = SearchCategoryByParentId("└", -1, ref listDropdowCate);
            var listItems = new List<SelectListItem>();
            string htmlcate = "";
            bool isCheck = false;
            foreach (var item in list)
            {
                isCheck = false;
                foreach (var re in CatalogProducts)
                {
                    if (item.Id == re.CatalogId) { isCheck = true; break; }
                }
                if (isCheck)
                {
                    if (catalogmain != item.Id)
                        htmlcate += "<label class=\"checkbox-inline\" style=\"width:83%\"><input id=\"cb_cata" + item.Id + "\" onclick=\"GetValueManuface(" + item.Id + ")\" value=\"" + item.Id + "\" type=\"checkbox\" name=\"catalogProducts\" checked>" + item.Name + "</label><span id=\"catalogProducts" + item.Id + "\"><a id=\"SetCatalogPrimary" + item.Id + "\" style=\"color:#428bca;margin-left:10px;\" onclick=\"SetCatalogPrimary(" + item.Id + ")\">Set Primary</a></span>";
                    else
                        htmlcate += "<label class=\"checkbox-inline\" style=\"width:83%\"><input id=\"cb_cata" + item.Id + "\" onclick=\"GetValueManuface(" + item.Id + ")\" value=\"" + item.Id + "\" type=\"checkbox\" name=\"catalogProducts\" checked>" + item.Name + "</label><span id=\"catalogProducts" + item.Id + "\"><span class=\"Primarycs\">Primary</span></span>";
                }
                else
                {
                    htmlcate += "<label class=\"checkbox-inline\" style=\"width:83%\"><input id=\"cb_cata" + item.Id + "\" onclick=\"GetValueManuface(" + item.Id + ")\" value=\"" + item.Id + "\" type=\"checkbox\" name=\"catalogProducts\">" + item.Name + "</label><span id=\"catalogProducts" + item.Id + "\"></span>";
                }
            }
            return htmlcate;
        }

        public List<SelectListItem> SearchCategoryByName(ref List<LoadDropdown.DropdowCate> listDropdowCate, long id)
        {
            var list = SearchCategoryByParentId("└-------", -1, ref listDropdowCate);
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "└Nhóm sản phẩm", Value = "-1", Selected = true });
            foreach (var item in list)
            {
                if (item.Id != id)
                {
                    listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
                else
                {
                    listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString(), Selected = true });
                }
            }
            return listItems;
        }

        public List<LoadDropdown.DropdowCate> SearchCategoryByParentId(string sp, long paId, ref List<LoadDropdown.DropdowCate> listDropdowCate)
        {
            List<Catalog> list = _catalogsBusiness.GetDynamicQuery().Where(x => x.ParentId == paId).ToList();
            //     List<Catalog> list = MemberController.List.Where(x => x.ParentId == paId).ToList();// Test cache dl
            //   List<Catalog> list = CategoryProductController.ListCate.Where(x => x.ParentId == paId).ToList();
            if (list.Any())
            {
                foreach (var catalog in list)
                {
                    var dropdowCate = new LoadDropdown.DropdowCate { Id = catalog.Id, Name = sp + catalog.CatalogName };
                    listDropdowCate.Add(dropdowCate);
                    SearchCategoryByParentId(sp + "--------", catalog.Id, ref listDropdowCate);
                }
            }

            return listDropdowCate;
        }

        public class DropdowCate
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        public List<SelectListItem> InitSelectListItemColWidth(int i = 2)
        {
            var listItemStatus = new List<SelectListItem>();
            //listItemStatus.Add(new SelectListItem { Text = "--Chọn trạng thái--", Value = "-1" });
            if (i == 2)
            {
                listItemStatus.Add(new SelectListItem { Text = "Độ rộng 2x", Value = "2", Selected = true });
                listItemStatus.Add(new SelectListItem { Text = "Độ rộng 5x", Value = "5" });
            }

            if (i == 5)
            {
                listItemStatus.Add(new SelectListItem { Text = "Độ rộng 2x", Value = "2" });
                listItemStatus.Add(new SelectListItem { Text = "Độ rộng 5x", Value = "5", Selected = true });
            }

            //listItemStatus.Add(new SelectListItem { Text = "Sản phẩm đã hết hạn", Value = "3" });
            //listItemStatus.Add(new SelectListItem { Text = "Sản phẩm đã hết hàng", Value = "4" });
            //listItemStatus.Add(new SelectListItem { Text = "Sản phẩm chờ bán (khóa)", Value = "5" });
            //listItemStatus.Add(new SelectListItem { Text = "Sản phẩm đã xóa", Value = "6" });
            return listItemStatus;
        }

        public List<SelectListItem> InitSelectListItemStatus()
        {
            var listItemStatus = new List<SelectListItem>();
            //listItemStatus.Add(new SelectListItem { Text = "--Chọn trạng thái--", Value = "-1" });
            listItemStatus.Add(new SelectListItem { Text = "Sản phẩm đang bán", Value = "2", Selected = true });
            listItemStatus.Add(new SelectListItem { Text = "Sản phẩm chưa duyệt", Value = "1" });

            //listItemStatus.Add(new SelectListItem { Text = "Sản phẩm đã hết hạn", Value = "3" });
            //listItemStatus.Add(new SelectListItem { Text = "Sản phẩm đã hết hàng", Value = "4" });
            //listItemStatus.Add(new SelectListItem { Text = "Sản phẩm chờ bán (khóa)", Value = "5" });
            //listItemStatus.Add(new SelectListItem { Text = "Sản phẩm đã xóa", Value = "6" });
            return listItemStatus;
        }

        public List<SelectListItem> InitSelectListItemState(int? s)
        {
            var listItemStatus = new List<SelectListItem>();
            if (s == null)
            {
                listItemStatus.Add(new SelectListItem { Text = "--Chọn trạng thái--", Value = "-1", Selected = true });
                listItemStatus.Add(new SelectListItem { Text = "Còn hàng", Value = "1" });
                listItemStatus.Add(new SelectListItem { Text = "Hết hàng", Value = "0" });
            }
            else
            {
                if (s == 1)
                {
                    listItemStatus.Add(new SelectListItem { Text = "--Chọn trạng thái--", Value = "-1" });
                    listItemStatus.Add(new SelectListItem { Text = "Còn hàng", Value = "1", Selected = true });
                    listItemStatus.Add(new SelectListItem { Text = "Hết hàng", Value = "0" });
                }
                else
                {
                    listItemStatus.Add(new SelectListItem { Text = "--Chọn trạng thái--", Value = "-1" });
                    listItemStatus.Add(new SelectListItem { Text = "Còn hàng", Value = "1" });
                    listItemStatus.Add(new SelectListItem { Text = "Hết hàng", Value = "0", Selected = true });
                }
            }
            return listItemStatus;
        }

        #region category

        public List<SelectListItem> InitDropCategorys(int islast)
        {
            var listItemStatus = new List<SelectListItem>();
            listItemStatus.Add(new SelectListItem { Text = "-Danh mục gốc-", Value = "-1" });

            List<Catalog> obj;

            obj = CategoryProductController.ListCate.Where(x => x.Status == 1 && x.IsLast == islast).ToList();

            foreach (var item in obj)
            {
                listItemStatus.Add(new SelectListItem { Text = item.CatalogName, Value = item.Id.ToString() });
            }
            return listItemStatus;
        }

        public List<SelectListItem> InitDropCategorysParent()
        {
            var listItemStatus = new List<SelectListItem>();
            listItemStatus.Add(new SelectListItem { Text = "-Select-", Value = "-1" });
            //    var obj =CategoryProductController.ListCate.Where(x => x.Status == 1 && x.ParentId == -1);
            List<Catalog> obj;

            obj = CategoryProductController.ListCate.Where(x => x.Status == 1 && x.ParentId == -1).ToList();

            foreach (var item in obj)
            {
                listItemStatus.Add(new SelectListItem { Text = item.CatalogName, Value = item.Id.ToString() });
            }
            return listItemStatus;
        }

        public List<SelectListItem> InitManufaceture()
        {
            var listItemStatus = new List<SelectListItem>();
            //    listItemStatus.Add(new SelectListItem { Text = "-Select-", Value = "-1" });
            //var obj = _manufacturersBusiness.GetDynamicQuery();
            var obj = CategoryProductController.ListManufacturers.OrderBy(x => x.ManufacturerName).ToList();
            foreach (var item in obj)
            {
                listItemStatus.Add(new SelectListItem { Text = item.ManufacturerName, Value = item.Id.ToString() });
            }
            return listItemStatus;
        }

        public List<SelectListItem> InitManufacetureByCateId(long cateId)
        {
            var listItemStatus = new List<SelectListItem>();
            //    listItemStatus.Add(new SelectListItem { Text = "-Select-", Value = "-1" });
            var obj = _catalogsManufacturersBusiness.GetDynamicQuery().Where(x => x.CatalogId == cateId);
            foreach (var item in obj)
            {
                listItemStatus.Add(new SelectListItem { Text = item.Manufacturer.ManufacturerName, Value = item.Id.ToString() });
            }
            return listItemStatus;
        }

        #region Commbo Create

        public List<SelectListItem> InitSelectListItemStatusCreateCategory()
        {
            var listItemStatus = new List<SelectListItem>();
            listItemStatus.Add(new SelectListItem { Text = "--Chọn trạng thái--", Value = "-1" });
            listItemStatus.Add(new SelectListItem { Text = "Lock", Value = "0" });
            listItemStatus.Add(new SelectListItem { Text = "Active", Value = "1" });

            return listItemStatus;
        }

        #endregion Commbo Create

        #region PromotionList Create

        public List<SelectListItem> InitSelectListPromotionList(long? promoID)
        {
            PromotionListBusiness promotionListBussiness = new PromotionListBusiness();
            var listPromotion = new List<PromotionList>();
            var listItemStatus = new List<SelectListItem>();
            if (promoID == null)
            {
                listItemStatus.Add(new SelectListItem { Text = "-Select-", Value = null, Selected = true });
            }
            else
            {
                listItemStatus.Add(new SelectListItem { Text = "-Select-", Value = null });
            }
            var obj = promotionListBussiness.GetDynamicQuery().ToList();
            foreach (var item in obj)
            {
                if (item.Id == promoID)
                    listItemStatus.Add(new SelectListItem { Text = item.Title, Value = item.Id.ToString(), Selected = true });
                else
                    listItemStatus.Add(new SelectListItem { Text = item.Title, Value = item.Id.ToString() });
            }
            return listItemStatus;
        }

        #endregion PromotionList Create

        #endregion category

        #region News

        #region NewsGroup

        public List<SelectListItem> InitSelectListItemStatusNewsGroup()
        {
            var listItemStatus = new List<SelectListItem>();
            listItemStatus.Add(new SelectListItem { Text = "--Chọn trạng thái--", Value = "-1" });

            var listNewsGroup = _newsGroupBusiness.GetListNewsGroup();
            foreach (NewsGroups item in listNewsGroup)
            {
                listItemStatus.Add(new SelectListItem { Text = item.NewsGroupName, Value = item.Id + "" });
            }

            return listItemStatus;
        }

        public List<SelectListItem> IsPublicForNewsGroup()
        {
            var listItemStatus = new List<SelectListItem>();
            listItemStatus.Add(new SelectListItem { Text = "--Chọn trạng thái--", Value = "-1" });
            listItemStatus.Add(new SelectListItem { Text = "public", Value = "0" });
            listItemStatus.Add(new SelectListItem { Text = "private", Value = "1" });

            return listItemStatus;
        }

        #endregion NewsGroup

        public List<SelectListItem> InitSelectListItemStatusNews()
        {
            var listItemStatus = new List<SelectListItem>();
            listItemStatus.Add(new SelectListItem { Text = "--Chọn trạng thái--", Value = "-1" });
            listItemStatus.Add(new SelectListItem { Text = "Lock", Value = "0" });
            listItemStatus.Add(new SelectListItem { Text = "Active", Value = "1" });

            return listItemStatus;
        }

        public List<SelectListItem> SearchNewsByName(ref List<DropdowNews> listDropdowNews)
        {
            var list = SearchNewsByParentId("└-------", -1, ref listDropdowNews);
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "└Nhóm tin", Value = "-1", Selected = true });
            foreach (var item in list)
            {
                listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return listItems;
        }

        public List<SelectListItem> SearchNewsByName(ref List<DropdowNews> listDropdowNews, long id)
        {
            var list = SearchNewsByParentId("└-------", -1, ref listDropdowNews);
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "└Nhóm tin", Value = "-1", Selected = true });
            foreach (var item in list)
            {
                if (item.Id != id)
                {
                    listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
                else
                {
                    listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString(), Selected = true });
                }
            }
            return listItems;
        }

        public List<DropdowNews> SearchNewsByParentId(string sp, long paId, ref List<DropdowNews> listDropdowNews)
        {
            List<NewsGroups> list = _newsGroupBusiness.GetDynamicQuery().Where(x => x.ParentId == paId).ToList();
            foreach (var newsGroup in list)
            {
                var dropdowCate = new DropdowNews { Id = newsGroup.Id, Name = sp + newsGroup.NewsGroupName };
                listDropdowNews.Add(dropdowCate);
                SearchNewsByParentId(sp + "--------", newsGroup.Id, ref listDropdowNews);
            }
            return listDropdowNews;
        }

        public class DropdowNews
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        #endregion News

        #region Shop

        public static List<SelectListItem> LoadDropShop()
        {
            var listShop = new ShopsBusiness().GetDynamicQuery().OrderBy(x => x.ShopName).ToList();
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "--Chọn shop--", Value = "-1", Selected = true });
            foreach (var item in listShop)
            {
                listItems.Add(new SelectListItem { Text = item.ShopName, Value = item.Id.ToString() });
            }
            return listItems;
        }

        public List<SelectListItem> LoadStatus()
        {
            var listStatus = new List<SelectListItem>();
            listStatus.Add(new SelectListItem { Text = "Khóa", Value = "0" });
            listStatus.Add(new SelectListItem { Text = "Active", Value = "1" });
            listStatus.Add(new SelectListItem { Text = "Đang duyệt", Value = "2" });
            return listStatus;
        }

        #endregion Shop

        #region Order

        public static List<SelectListItem> LoadDropOrder(int? flag)
        {
            if (flag == null || flag == -1)
            {
                var listItemStatus = new List<SelectListItem>
            {
                new SelectListItem {Text = "-- Chọn trạng thái --", Value = "-1"},
                new SelectListItem {Text = "Đơn hàng mới", Value = "0"},
                new SelectListItem {Text = "Đơn hàng chờ giao dịch", Value = "1"},
                new SelectListItem {Text = "Chờ chuyển hàng", Value = "2"},
                new SelectListItem {Text = "Đã hoàn tất", Value = "3"},
                new SelectListItem {Text = "Hủy giao dịch", Value = "4"}
            };
                return listItemStatus;
            }
            else
            {
                var listItemStatus = new List<SelectListItem>();
                listItemStatus.Add(new SelectListItem { Text = "--Trạng thái--", Value = "-1" });
                listItemStatus.Add(flag == 1
                    ? new SelectListItem { Text = "Đơn hàng mới", Value = "0", Selected = true }
                    : new SelectListItem { Text = "Đơn hàng mới", Value = "0" });
                listItemStatus.Add(flag == 2
               ? new SelectListItem { Text = "Đơn hàng chờ giao dịch", Value = "1", Selected = true }
               : new SelectListItem { Text = "Đơn hàng chờ giao dịch", Value = "1" });

                listItemStatus.Add(flag == 2
                   ? new SelectListItem { Text = "Chờ chuyển hàng", Value = "2", Selected = true }
                   : new SelectListItem { Text = "Chờ chuyển hàng", Value = "2" });

                listItemStatus.Add(flag == 3
                  ? new SelectListItem { Text = "Đã hoàn tất", Value = "3", Selected = true }
                  : new SelectListItem { Text = "Đã hoàn tất", Value = "3" });
                listItemStatus.Add(flag == 4
                ? new SelectListItem { Text = "Hủy giao dịch", Value = "4", Selected = true }
                : new SelectListItem { Text = "Hủy giao dịch", Value = "4" });
                return listItemStatus;
            }
        }

        public static List<SelectListItem> LoadDropOrderPaid(int? state)
        {
            if (state == null || state == -1)
            {
                var listItemStatus = new List<SelectListItem>
            {
                new SelectListItem {Text = "-- Chọn trạng thái --", Value = "-1"},
                new SelectListItem {Text = "Chưa thanh toán", Value = "0"},
                new SelectListItem {Text = "Đã thanh toán", Value = "1"},
            };
                return listItemStatus;
            }
            else
            {
                var listItemStatus = new List<SelectListItem>();
                if (state == 1)
                {
                    listItemStatus.Add(new SelectListItem { Text = "--Chọn trạng thái--", Value = "-1" });
                    listItemStatus.Add(new SelectListItem { Text = "Đã thanh toán", Value = "1", Selected = true });
                    listItemStatus.Add(new SelectListItem { Text = "Chưa thanh toán", Value = "0" });
                }
                else
                {
                    listItemStatus.Add(new SelectListItem { Text = "--Chọn trạng thái--", Value = "-1" });
                    listItemStatus.Add(new SelectListItem { Text = "Đã thanh toán", Value = "1" });
                    listItemStatus.Add(new SelectListItem { Text = "Chưa thanh toán", Value = "0", Selected = true });
                }
                return listItemStatus;
            }
        }

        #endregion Order

        public class DropdowMenus
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        public List<SelectListItem> SearchMenusByName(ref List<DropdowMenus> listDropdowMenus)
        {
            var list = SearchMenusByParentId("└-------", -1, ref listDropdowMenus);
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "└Menu Cha", Value = "-1", Selected = true });
            foreach (var item in list)
            {
                listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return listItems;
        }

        public List<SelectListItem> SearchMenusByName(ref List<DropdowMenus> listDropdowMenus, long id)
        {
            var list = SearchMenusByParentId("└-------", -1, ref listDropdowMenus);
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "└Menu cha", Value = "-1", Selected = true });
            foreach (var item in list)
            {
                if (item.Id != id)
                {
                    listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
                else
                {
                    listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString(), Selected = true });
                }
            }
            return listItems;
        }

        public List<DropdowMenus> SearchMenusByParentId(string sp, long paId, ref List<DropdowMenus> listDropdowMenus)
        {
            List<Menus> list = _menuBusiness.GetDynamicQuery().Where(x => x.ParentId == paId).ToList();
            foreach (var menus in list)
            {
                var dropdowCate = new DropdowMenus { Id = menus.Id, Name = sp + menus.MenuName };
                listDropdowMenus.Add(dropdowCate);
                SearchMenusByParentId(sp + "--------", menus.Id, ref listDropdowMenus);
            }
            return listDropdowMenus;
        }

        #region CatalogDownloads

        public class DropdowCatalogDownloads
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        public List<SelectListItem> SearchCatalogDownloadByName(ref List<DropdowCatalogDownloads> listDropdowdownload)
        {
            var list = SearchDownloadsByParentId("└-------", -1, ref listDropdowdownload);
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "└Nhóm tài liệu", Value = "-1", Selected = true });
            foreach (var item in list)
            {
                listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return listItems;
        }

        public List<SelectListItem> SearchCatalogDownloadByName(ref List<DropdowCatalogDownloads> listDropdowNews, long id)
        {
            var list = SearchDownloadsByParentId("└-------", -1, ref listDropdowNews);
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "└Nhóm tài liệu", Value = "-1", Selected = true });
            foreach (var item in list)
            {
                if (item.Id != id)
                {
                    listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
                else
                {
                    listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString(), Selected = true });
                }
            }
            return listItems;
        }

        public List<DropdowCatalogDownloads> SearchDownloadsByParentId(string sp, long paId, ref List<DropdowCatalogDownloads> listDropdowDownloads)
        {
            List<CatalogDownloads> list = _CatalogDownloadBusiness.GetDynamicQuery().Where(x => x.ParentId == paId).ToList();
            foreach (var catalogDownloads in list)
            {
                var dropdowCate = new DropdowCatalogDownloads { Id = catalogDownloads.Id, Name = sp + catalogDownloads.CatalogDownloadName };
                listDropdowDownloads.Add(dropdowCate);
                SearchDownloadsByParentId(sp + "--------", catalogDownloads.Id, ref listDropdowDownloads);
            }
            return listDropdowDownloads;
        }

        #endregion CatalogDownloads
    }
}