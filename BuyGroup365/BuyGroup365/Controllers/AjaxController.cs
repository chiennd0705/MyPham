using Business.ClassBusiness;
using BuyGroup365.Models.Member;
using Common;
using Common.util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BuyGroup365.Controllers
{
    public class AjaxController : Controller
    {
        //
        // GET: /Home/
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult GetProduct(string type, string info)
        {
            BuyGroup365Entities entitis = new BuyGroup365Entities();
            string priceDis;
            string[] splitImg;
            int intType = 1;
            if (type.Equals("featured")) intType = 1;
            else if (type.Equals("newproduct")) intType = 2;
            else if (type.Equals("saleproduct")) intType = 5;
            long CatalogId = 0;
            try
            {
                CatalogId = long.Parse(info);
            }
            catch { }

            var listProduct = entitis.SearchProductByType(intType, CatalogId, 1, 12);

            String html = "<div class=\"mage-magictabs mc-" + type + "\">" +

                                          "<ul class=\"flexisel-content products-grid featured zoomOut play\">";
            foreach (var item in listProduct)
            {
                html += "<li class=\"item item-animate\">" +
                         "<div class=\"per-product\">" +
                             "<div class=\"images-container\">" +
                                 "<div class=\"product-hover\">";
                if (item.IsVip) html += "<span class=\"sticker top-left\"><span class=\"labelvip\">Vip</span></span>";
                else if (item.IsAttractive) html += "<span class=\"sticker top-left\"><span class=\"labelnew\">Hot</span></span>";

                html += "<a href=\"" + Common.util.Function.InitUrlDetaiProduct(item.FriendlyUrl, item.Id) + "\" title=\"\" class=\"product-image\">" +
                                         "<img class=\"img-responsive\" src=\"";
                splitImg = item.ImgSource.Split('?');

                html += splitImg[0] + "Small/" + splitImg[1] + "\" alt=\"" + item.ProductName + "\">" +

                         "</a>" +
                     "</div>" +
                     "<div class=\"actions-no hover-box\">" +
                         "<a class=\"detail_links\" href=\"" + Common.util.Function.InitUrlDetaiProduct(item.FriendlyUrl, item.Id) + "\"></a>" +
                         "<div class=\"actions\">" +
                             "<ul class=\"add-to-links pull-left-none\">" +
                                 "<li class=\"pull-left-none\"><a href=\"javaScript: void(0);\" onclick=\"SetWishlist(" + item.Id + ")\" title=\"Add to Wishlist\" class=\"link-wishlist\"><i class=\"fa fa-heart-o icons\"></i>Add to Wishlist</a></li>" +

                                 "<li class=\"pull-left-none\"><span class=\"separator\">|</span> <a href=\"javaScript: void(0);\" onclick=\"CompareProduct(" + item.Id + ")\" title=\"Add to Compare\" class=\"link-compare\"><i class=\"fa fa-signal icons\"></i>Add to Compare</a></li>" +
                                 "<li class=\"link-view pull-left-none\">" +
                                     "<a title=\"Quick View\" href=\"http://buygroup365.com/Ajax/QuickView/" + item.Id + "\" class=\"link-quickview\"><i class=\"fa fa-search icons\"></i>Quick View</a>" +
                                 "</li>" +
                             "</ul>" +
                         "</div>" +
                     // "<div class=\"actions-cart\">" +
                     //     "<button type=\"button\" title=\"Add to Cart\" class=\"button btn-cart pull-left-none\" onclick=\"\" magiccartevent=\"http://buygroup365.com/Ajax/QuickView/4\"><span><span>Add to Cart</span></span></button>" +
                     // "</div>" +
                     "</div>" +
                 "</div>" +
                 "<div class=\"products-textlink clearfix\">" +
                     "<h2 class=\"product-name\">" +
                         "<a href=\"" + Common.util.Function.InitUrlDetaiProduct(item.FriendlyUrl, item.Id) + "\" title=\"\">" + item.ProductName + "</a>" +
                     "</h2>";

                priceDis = string.Empty;
                if (item.Cost > item.Price) priceDis = "<p class=\"old-price\"><span class=\"price\">" + item.Cost.ToString("N0") + "<sup class=\"u-price\">đ</sup></span></p>";
                html += "<div class=\"price-box\">" + priceDis + "<p class=\"special-price\"><span class=\"price\" itemprop=\"price\">" + item.Price.ToString("N0") + "<sup class=\"u-price\">đ</sup></span></p></div>" +
                        "<div class=\"ratings\"><div class=\"rating-box\"><div class=\"rating\" style=\"width:" + InitStar(item.Rate) + "%\"></div></div></div>" +
                        "</div></div> </li>";
            }
            html += "</ul></div></div>";
            Response.Write(html.ToString());
            return null;
        }

        protected string InitStar(double? diem)
        {
            if (diem == 1) return "20";
            if (diem == 2) return "40";
            if (diem == 3) return "60";
            if (diem == 4) return "80";
            return "100";
        }

        public ActionResult QuickView(long id)
        {
            ProductsBusiness productBusiness = new ProductsBusiness();
            Product item = productBusiness.GetById(id);
            ViewData["ProductName"] = item.ProductName;

            CommentsBusiness commentBusiness = new CommentsBusiness();
            int count = 0;
            double rate;
            rate = commentBusiness.CalculatorByProductId(id, ref count);
            ViewData["rating"] = "<div class=\"rating\" style=\"width: " + InitStar(rate) + "%\"></div>";
            @ViewData["count"] = count;
            if (item.Cost > item.Price)
                ViewData["OldPrice"] = item.Cost.ToString("N0") + "<sup class=\"u-price\">đ</sup>";

            ViewData["Price"] = item.Price.ToString("N0") + "<sup class=\"u-price\">đ</sup>";

            if (item.State == 1) ViewData["State"] = "Còn hàng";
            else ViewData["State"] = "Hết hàng";

            ViewData["Summary"] = item.Summary;

            ProductImagesBusiness productImageBusiness = new ProductImagesBusiness();

            var listImg = productImageBusiness.FindProductId(id);
            string strImg1 = string.Empty, strImg2 = string.Empty;
            int dem = 0;
            foreach (ProductImage img in listImg)
            {
                if (img.IsAvatar == 1)
                {
                    ViewData["ImgMain"] = img.ImgSource.Split('?')[0] + "Medium/" + img.ImgSource.Split('?')[1];
                    ViewData["ImgMainBig"] = img.ImgSource.Split('?')[0] + "Large/" + img.ImgSource.Split('?')[1];
                }

                strImg1 += " <img id=\"image-" + dem + "\" class=\"gallery-image\" src=\"" + img.ImgSource.Split('?')[0] + "Medium/" + img.ImgSource.Split('?')[1] + "\" data-zoom-image=\"" + img.ImgSource.Split('?')[0] + "big_" + img.ImgSource.Split('?')[1] + "\">";
                strImg2 += "<li><a class=\"thumb-link\" href=\"#\" title=\"\" data-image-index=\"" + dem + "\">  <img class=\"img-responsive\" src=\"" + img.ImgSource.Split('?')[0] + "Small/" + img.ImgSource.Split('?')[1] + "\" alt=\"\"></a></li>";
                dem++;
            }
            ViewData["Img1"] = strImg1;
            ViewData["Img2"] = strImg2;
            return PartialView(item);
        }

        public JsonResult ViewCart(long id, int sl)
        {
            var entity = NlCheckout.GetSessionCard(Session);
            List<CartItem> listitems = new List<CartItem>();
            if (entity != null)
            {
                var booll = entity.Any(x => x.Product.Id == id);
                if (booll)
                {
                    foreach (var item in entity)
                    {
                        if (item.Product.Id == id)
                        {
                            item.Quantity = 1;
                            listitems.Add(item);
                        }
                        else
                        {
                            listitems.Add(item);
                        }
                    }
                    //     listitems.AddRange(entity);
                }
                else
                {
                    CartItem cart = new CartItem();
                    cart.Quantity = sl;
                    var product = new ProductsBusiness().GetById(id);
                    cart.Product = product;
                    cart.AvatarUrl = BuyGroup365.Models.Member.FuntionMember.GetUrlImage(product.ProductImages.First(x => x.IsAvatar == 1).ImgSource, "Small");
                    listitems.Add(cart);
                    listitems.AddRange(entity);
                }

                //  listitems.AddRange(entity);
            }
            else
            {
                CartItem cart = new CartItem();
                cart.Quantity = sl;
                var product = new ProductsBusiness().GetById(id);
                cart.Product = product;
                try
                {
                    cart.AvatarUrl = BuyGroup365.Models.Member.FuntionMember.GetUrlImage(product.ProductImages.First(x => x.IsAvatar == 1).ImgSource, "Small");
                }
                catch { }
                listitems.Add(cart);
            }
            //var objlist = listitems.Where(x => x.Product.Id == id);
            //if (objlist.Any())
            //{
            //}
            //CartItem cart =new CartItem();
            //cart.Quantity = sl;
            //var product = new ProductsBusiness().GetById(id);
            //cart.Product = product;
            //listitems.Add(cart);
            NlCheckout.SetSessionCard(listitems, Session);
            List<ProductCart> listpProductCarts = new List<ProductCart>();
            foreach (var item in listitems)
            {
                ProductCart productCart = new ProductCart();
                productCart.Id = item.Product.Id;
                productCart.Model = item.Product.Model;
                productCart.MemberId = item.Product.MemberId;
                productCart.ShopCatalogId = item.Product.CatalogId;
                productCart.ProductName = item.Product.ProductName;
                productCart.Code = item.Product.Code;
                productCart.FriendlyUrl = item.Product.FriendlyUrl;
                productCart.Summary = item.Product.Summary;
                productCart.Description = item.Product.Description;
                productCart.Price = item.Product.Price;
                productCart.Cost = item.Product.Cost;
                productCart.Weight = item.Product.Weight;
                productCart.Quantity = item.Quantity;
                productCart.AvatarUrl = item.AvatarUrl;
                listpProductCarts.Add(productCart);
            }
            return Json(listpProductCarts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBrandShowcase(string type, string info)
        {
            string priceDis;
            string[] splitImg;
            long brandId = long.Parse(type);

            ManufacturersBusiness manufacturersBusiness = new ManufacturersBusiness();

            Manufacturer brand = manufacturersBusiness.GetById(brandId);

            BuyGroup365Entities entitis = new BuyGroup365Entities();

            List<SearchProductByBrand_Result> listProduct = entitis.SearchProductByBrand(brandId, 1, 4).ToList();

            String html = "<div class=\"mage-magictabs mc-" + type + "\">" +
                          "<div class=\"row\"><div class=\"col-xs-12 col-sm-4 trademark-info\">" +
                          "<div class=\"brand-info\"><div class=\"brand-description\"><div class=\"trademark-logo\">" +
                           "<img alt=\"\" src=\"" + brand.Icon + "\"></div>" +
                           "<div class=\"trademark-desc\">" + brand.Description + "</div></div>" +
                           "<a class=\"trademark-link\" href=\"/search-dm0?mid=" + brandId + "\">Xem toàn bộ sản phẩm</a></div> </div> <div class=\"col-xs-12 col-sm-8 trademark-product\">" +
                           "<ul class=\"flexisel-content products-grid featured zoomOut play\">";
            foreach (var item in listProduct)
            {
                html += "<li class=\"item item-animate\">" +
                        "<div class=\"per-product\">" +
                            "<div class=\"images-container\">" +
                                "<div class=\"product-hover\">";
                if (item.IsVip) html += "<span class=\"sticker top-left\"><span class=\"labelvip\">Vip</span></span>";
                else if (item.IsAttractive) html += "<span class=\"sticker top-left\"><span class=\"labelnew\">Hot</span></span>";

                html += "<a href=\"" + Common.util.Function.InitUrlDetaiProduct(item.FriendlyUrl, item.Id) + "\" title=\"\" class=\"product-image\">" +
                                         "<img class=\"img-responsive\" src=\"";

                splitImg = item.ImgSource.Split('?');

                html += splitImg[0] + "Medium/" + splitImg[1] + "\" width=\"300\" height=\"366\" alt=\"\">" +

                         "</a>" +
                     "</div>" +

                 "</div>" +
                 "<div class=\"products-textlink clearfix\">" +
                     "<h2 class=\"product-name\">" +
                         "<a href=\"" + Common.util.Function.InitUrlDetaiProduct(item.FriendlyUrl, item.Id) + "\" title=\"\">" + item.ProductName + "</a>" +
                     "</h2>";

                priceDis = string.Empty;
                if (item.Cost > item.Price) priceDis = "<p class=\"old-price\"><span class=\"price\">" + item.Cost.ToString("N0") + "<sup class=\"u-price\">đ</sup></span></p>";
                html += "<div class=\"price-box\">" + priceDis + "<p class=\"special-price\"><span class=\"price\" itemprop=\"price\">" + item.Price.ToString("N0") + "<sup class=\"u-price\">đ</sup></span></p></div>" +
                        "<div class=\"ratings\"><div class=\"rating-box\"><div class=\"rating\" style=\"width:" + InitStar(item.Rate) + "%\"></div></div></div>" +
                        "</div></div> </li>";
            }

            Response.Write(html.ToString());
            return null;
        }

        //********Cart Item******
        public JsonResult DeleteCart(long id)
        {
            List<CartItem> listresults = new List<CartItem>();
            var listObj = NlCheckout.GetSessionCard(Session);
            foreach (var cartItem in listObj)
            {
                if (cartItem.Product.Id != id)
                {
                    listresults.Add(cartItem);
                }
            }
            NlCheckout.SetSessionCard(listresults, Session);
            return Json(listresults, JsonRequestBehavior.AllowGet);
        }

        //End
        //********Quan tâm của tôi******
        public JsonResult AddWishlist(long id)
        {
            var obj = SessionUtility.GetSessionMember(Session);
            if (obj == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);//Bạn phải đăng nhập để thực hiện chức năng này.
            }
            else
            {
                var memberCareProductBusiness = new MemberCareProductBusiness();
                var listCare =
                    memberCareProductBusiness.GetDynamicQuery()
                        .Where(x => x.MemberId == obj.Id && x.ProductId == id)
                        .ToList();
                if (listCare.Any())
                {
                    return Json(1, JsonRequestBehavior.AllowGet);//Sản phẩm đã có trong danh sách quan tâm của bạn
                }
                else
                {
                    var entity = new MemberCareProduct { MemberId = obj.Id, ProductId = id };
                    memberCareProductBusiness.AddNew(entity);
                    return Json(2, JsonRequestBehavior.AllowGet);//Sản phẩm đã được thêm vào danh sách quan tâm của bạn
                }
            }
        }

        //End

        #region Add comment

        public JsonResult AddComment(string array)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            var arr = JsonConvert.DeserializeObject<List<string>>(array, serializerSettings);
            //var arr = array.Split(',');
            Comment comment = new Comment();
            comment.NickName = arr[0];
            comment.Rate = int.Parse(arr[2]);
            comment.Content = arr[1];
            comment.ProductId = long.Parse(arr[3]);
            comment.ParentId = -1;
            var obj = SessionUtility.GetSessionMember(Session);
            if (obj != null)
            {
                comment.MemberId = obj.Id;
            }
            else
            {
                comment.MemberId = -1;
            }
            var product = new ProductsBusiness().GetById(comment.ProductId);
            comment.ShopId = product.MemberId;
            new CommentsBusiness().AddNew(comment);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        #endregion Add comment

        #region Cooki Add comparse

        public ActionResult ViewComparse()
        {
            return View();
        }

        public JsonResult CompareCookie(string productId)
        {
            var mycookies = new NlCheckout().GetvalueAppsetting("my_cookies");
            var timecookies = new NlCheckout().GetvalueAppsetting("time_cookies");
            var limits = new NlCheckout().GetvalueAppsetting("max_cookies");
            var obj = new ProductsBusiness().GetById(long.Parse(productId));
            var rate = obj.Comments.Where(x => x.ProductId == obj.Id).ToList();
            int star = 0;
            if (rate.Any())
            {
                var a = rate.Select(x => x.Rate).Max();
                if (a == 1)
                {
                    star = 20;
                }
                else if (a == 2)
                {
                    star = 40;
                }
                else if (a == 3)
                {
                    star = 60;
                }
                else if (a == 4)
                {
                    star = 80;
                }
                else
                {
                    star = 100;
                }
            }
            var objItem = new ProductItem
            {
                Id = obj.Id,
                Img1 = obj.ProductImages.First(x => x.IsAvatar == 1).ImgSource,
                Price = obj.Price,
                ProductName = obj.ProductName,
                Cost = obj.Cost,
                FriendlyUrl = obj.FriendlyUrl,
                Rate = star,
                TotalView = obj.TotalView
            };

            try
            {
                var objcookies = Common.util.SessionUtility.ReadCookie(mycookies);

                var arr = new List<ProductItem>();
                if (objcookies != null)
                {
                    arr = JsonConvert.DeserializeObject<List<ProductItem>>(objcookies);
                }
                List<ProductItem> strArr = new List<ProductItem>();
                if (arr.Any())
                {
                    if (arr.Count > int.Parse(limits))
                    {
                        return Json(2, JsonRequestBehavior.AllowGet);//Quá sô lượng đối tượng lưu trong cookies
                    }
                    else
                    {
                        if (!arr.Select(x => x.Id).Contains(objItem.Id))
                        {
                            arr.Add(objItem);
                        }
                        strArr.AddRange(arr);
                    }
                }
                else
                {
                    strArr.Add(objItem);
                }
                Common.util.SessionUtility.WriteCookie(mycookies, strArr, int.Parse(timecookies));
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCookies()
        {
            var mycookies = new NlCheckout().GetvalueAppsetting("my_cookies");
            var objcookies = Common.util.SessionUtility.ReadCookie(mycookies);
            var arr = new List<Common.util.ProductItem>();
            if (objcookies != null)
            {
                arr = JsonConvert.DeserializeObject<List<Common.util.ProductItem>>(objcookies);
            }
            if (arr.Any())
            {
                string body = ControllerExtensions.RenderRazorViewToString(this, "ViewComparse", arr);
                return Json(body, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SendBaoGia(string Email)
        {
            bool check = false;
            Common.util.Function.ObjMailSend ojbmail = new Common.util.Function.ObjMailSend();
            ojbmail.ToMail = System.Configuration.ConfigurationManager.AppSettings["ToMail"];
            ojbmail.FromMail = System.Configuration.ConfigurationManager.AppSettings["FromMail"];
            ojbmail.PassFromMail = System.Configuration.ConfigurationManager.AppSettings["PassFromMail"];
            try
            {
                string title = "Thông tin đăng ký nhận báo giá";
                string body = "Đăng ký nhận thông tin<br/>" +

                     "<b>Email:</b>" + Email + "<br/>";
                Common.util.Function.email_send(ojbmail, title, body);

                check = true;
            }
            catch
            {
                check = false;
            }
            return Json(new
            {
                data = true,
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion Cooki Add comparse

        #region FAQ

        public JsonResult SubmitFaq(long id, string Name, string Email, string Content)
        {
            bool check = false;
            try
            {
                CommentsNewsBusiness commentbusiness = new CommentsNewsBusiness();
                Common.CommentsNew comment = new Common.CommentsNew();
                comment.NewId = id;
                comment.Content = Content;
                comment.CreateDate = DateTime.Now;
                comment.Email = Email;
                comment.Status = 1;
                comment.NickName = Name;
                commentbusiness.AddNew(comment);
                check = true;
            }
            catch
            {
                check = false;
            }
            return Json(new
            {
                data = check,
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion FAQ

        #region Downloads

        public JsonResult ListDownload(long id, string Searchtext, int size)
        {
            DownloadsBusiness _downloadBussiness = new DownloadsBusiness();
            List<Downloads> ListDowns = new List<Downloads>();
            List<Download> ListDown = new List<Download>();
            ListDowns = _downloadBussiness.GetListID(id, Searchtext, size);
            foreach (var tem in ListDowns)
            {
                Download dl = new Download();
                dl.CatalogDownloadID = tem.CatalogDownloadID;
                // dl.CreateDate = tem.CreateDate;
                dl.Description = tem.Description;
                dl.FileName = tem.FileName;
                dl.Modifydate = tem.Modifydate.ToString("dd/M/yyyy");
                dl.Size = tem.Size.Value;
                dl.SoureFile = tem.SoureFile;
                ListDown.Add(dl);
            }

            return Json(ListDown, JsonRequestBehavior.AllowGet);
        }

        #endregion Downloads

        #region News

        public JsonResult GetNewHome(long id)
        {
            Business.ClassBusiness.NewsBusiness newsBusiness = new Business.ClassBusiness.NewsBusiness();
            List<Common.News> listNews = newsBusiness.ListByNewsIdNewsGroupActive(id, 2);

            List<ModelNews> ListNew = new List<ModelNews>();

            foreach (var tem in listNews)
            {
                ModelNews dl = new ModelNews();
                dl.Title = tem.Title;
                // dl.CreateDate = tem.CreateDate;
                dl.Summary = tem.Summary;
                dl.NewsGroupId = tem.NewsGroupId;
                dl.ModifyDate = tem.ModifyDate.ToString("dd/M/yyyy");
                dl.Descriptions = tem.Descriptions;
                dl.ImageSource = tem.ImageSource;
                dl.Id = tem.Id;
                dl.FriendlyUrl = tem.FriendlyUrl;
                ListNew.Add(dl);
            }

            return Json(ListNew, JsonRequestBehavior.AllowGet);
        }

        #endregion News
    }
}