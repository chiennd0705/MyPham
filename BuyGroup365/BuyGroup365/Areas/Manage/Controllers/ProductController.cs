using Business.ClassBusiness;
using BuyGroup365.Areas.Manage.Models;
using BuyGroup365.Controllers;
using BuyGroup365.Models.Common;
using BuyGroup365.Models.Member;
using Common;
using Common.util;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class ProductController : BaseController
    {
        #region Khai bao biến

        private ProductsBusiness _productsBusiness = new ProductsBusiness();
        private ProductImagesBusiness _productImagesBusiness = new ProductImagesBusiness();
        private ProductPropertiesBusiness _productPropertiesBusiness = new ProductPropertiesBusiness();
        private Catalogs_ManufacturersBusiness _catalogsManufacturersBusiness = new Catalogs_ManufacturersBusiness();
        private CatalogsBusiness _catalogsBusiness = new CatalogsBusiness();
        private CatalogPropertiesBusiness _catalogPropertiesBusiness = new CatalogPropertiesBusiness();
        private CatalogPropertiesValueBusiness _catalogPropertiesValueBusiness = new CatalogPropertiesValueBusiness();
        private readonly FriendlyUrlBusiness _friendlyUrlBusines = new FriendlyUrlBusiness();
        private readonly LoadCombo _loadCombo = new LoadCombo();
        private readonly CommentsBusiness _commentsBusiness = new CommentsBusiness();
        private CatalogProductsBusiness _catalogProduct = new CatalogProductsBusiness();
        private string duoilink = System.Configuration.ConfigurationSettings.AppSettings["ExtensionLink"];
        //  public static List<Catalog> List = null;

        #endregion Khai bao biến

        //
        // GET: /Manage/Product/
        public ProductController()
        {
            if (CategoryProductController.ListCate == null)
            {
                CategoryProductController.ListCate = _catalogsBusiness.GetDynamicQuery().ToList();
            }
            if (MemberController.ListCatalogsManufacturerses == null)
            {
                MemberController.ListCatalogsManufacturerses = _catalogsManufacturersBusiness.GetDynamicQuery().ToList();
            }
            if (MemberController.List == null)
            {
                MemberController.List = _catalogsBusiness.GetDynamicQuery().ToList();
            }
        }

        public ActionResult Index(string name, int? page, long? shop = -1, int? statusProduct = -1, int? isOfProduct = -1, int ListCatalogProduct = -1)
        {
            try
            {
                var obj = HtmlCate(-1);
                ViewBag.htmlCate = obj;
                ViewBag.categoryproduct = _loadCombo.InitDropCategorys(1);
                ViewBag.categoryproductedit = _loadCombo.InitDropCategorys(1);
                ViewBag.categoryproductParent = _loadCombo.InitDropCategorysParent();
                ViewBag.statusProduct = _loadCombo.InitSelectListItemStatus();
                ViewBag.stateProduct = _loadCombo.InitSelectListItemState(null);
                List<LoadDropdown.DropdowCate> listDropdowCate = new List<LoadDropdown.DropdowCate>();

                ViewBag.ListCatalogProduct = _loadCombo.SearchCategoryByName(ref listDropdowCate);

                ViewBag.shop = new BuyGroup365.Models.Common.Util().LoadComBoShop();
                //cap nhat trang thai don hang
                ViewBag.statusOfProduct = InitSelectListItemStatusProduct();
                ViewBag.isOfProduct = InitSelectListItemIsOfProduct();

                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = _productsBusiness.GetByKey(name, statusProduct, isOfProduct, shop, ListCatalogProduct).OrderByDescending(p => p.ModifyDate).ToPagedList(currentPageIndex, 20);
                ViewBag.ShopId = shop;
                ViewBag.Name = name;
                ViewBag.Statusp = statusProduct;
                ViewBag.States = isOfProduct;
                ViewBag.Page = currentPageIndex;

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

        public List<SelectListItem> InitSelectListItemStatusProduct()
        {
            var listItemStatus = new List<SelectListItem>();
            listItemStatus.Add(new SelectListItem { Text = "--Chọn trạng thái--", Value = "-1" });
            listItemStatus.Add(new SelectListItem { Text = "Sản phẩm chưa được duyệt", Value = "1" });
            listItemStatus.Add(new SelectListItem { Text = "Sản phẩm đang bán(đã được duyệt)", Value = "2" });
            listItemStatus.Add(new SelectListItem { Text = "Sản phẩm đã hết hạn", Value = "3" });
            listItemStatus.Add(new SelectListItem { Text = "Sản phẩm đã hết hàng", Value = "4" });
            listItemStatus.Add(new SelectListItem { Text = "Sản phẩm chờ bán(bị tạm khóa)", Value = "5" });

            return listItemStatus;
        }

        public List<SelectListItem> InitSelectListItemIsOfProduct()
        {
            var listItemStatus = new List<SelectListItem>();
            listItemStatus.Add(new SelectListItem { Text = "--Chọn trạng thái--", Value = "-1" });
            listItemStatus.Add(new SelectListItem { Text = "IsNew", Value = "1" });
            listItemStatus.Add(new SelectListItem { Text = "IsAttactive", Value = "2" });
            listItemStatus.Add(new SelectListItem { Text = "IsVip", Value = "3" });

            return listItemStatus;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(string name, string model, string friendlyurl, long categoryproduct,
            long manufaceproduct, float pice, float cost, float weight, string typeofcurrency, string tags, int statusProduct, int isnew, int isattactive, int isvip,
            HttpPostedFileBase avatar, int stateProduct, string summary, string description, HttpPostedFileBase[] file, string[] proprertyvalue)
        {
            try
            {
                var objproduct = new Product();
                objproduct.ProductName = name;
                objproduct.Code = "ccc";
                objproduct.Model = model;
                objproduct.Cost = cost;
                objproduct.CatalogId = categoryproduct;
                objproduct.Description = description;
                objproduct.FriendlyUrl = friendlyurl;
                objproduct.Tags = Function.ConvertFileNameSpace(name) + ", " +
                                  new CatalogsBusiness().GetById(categoryproduct).CatalogName + ", " +
                                  Function.ConvertFileNameSpace(new CatalogsBusiness().GetById(categoryproduct).CatalogName);
                objproduct.Summary = summary;
                if (isnew == 1)
                {
                    objproduct.IsNew = true;
                }
                if (isattactive == 1)
                {
                    objproduct.IsAttractive = true;
                }
                if (isvip == 1)
                {
                    objproduct.IsVip = true;
                }
                objproduct.ManufacturerId = manufaceproduct;
                objproduct.MemberId = 0;//Fix
                objproduct.Price = pice;
                objproduct.State = stateProduct;
                objproduct.Status = statusProduct;
                objproduct.TypeOfCurrency = typeofcurrency;
                objproduct.Weight = weight;

                #region Property

                var productProperties = new List<ProductProperty>();
                var listobjproperty = proprertyvalue.ToList();
                foreach (var item in listobjproperty)
                {
                    var splitobj = item.Split('|');
                    var cateproperty = _catalogPropertiesBusiness.GetById(long.Parse(splitobj[0]));
                    for (int i = 1; i < splitobj.Count(); i++)
                    {
                        var productProperty = new ProductProperty();
                        productProperty.Name = cateproperty.Name;
                        productProperty.Key = cateproperty.Key;
                        productProperty.Value = splitobj[i];
                        productProperties.Add(productProperty);
                    }
                }
                //for (int i = 1; i <= index; i++)
                //{
                //  var  productProperty=new ProductProperty();
                //    var namepro = formCollections["nameproperty"+i];
                //    var keypro = formCollections["keyproperty"+i];
                //    var valpro = formCollections["valeproperty"+i];
                //    productProperty.Name = namepro;
                //    productProperty.Key = keypro;
                //    productProperty.Value = valpro;
                //    productProperties.Add(productProperty);
                //}

                #endregion Property

                #region image

                List<ProductImage> productImages = new List<ProductImage>();
                if (file != null)
                {
                    foreach (var item in file)
                    {
                        if (item != null && item.ContentLength > 0)
                        {
                            var check = Util.CheckFileImage(item);
                            if (check == 0)
                            {
                                ViewBag.Mes = "File quá lớn";
                                return RedirectToAction("Index");
                            }
                            else if (check == 1)
                            {
                                ViewBag.Mes = "File không đúng định dạng";
                                return RedirectToAction("Index");
                            }
                            ProductImage productImage = new ProductImage();
                            // TourInfo entity=new TourInfo();
                            //Random rdImage = new Random();
                            string randomImage = Guid.NewGuid().ToString();
                            string fileNameImage = Common.util.Function.ConvertFileName(item.FileName);
                            string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                            var strurlimage = Common.util.Function.ResizeImageNew(item, 300, 300, pathImage, randomImage);
                            Common.util.Function.ResizeImageNew(item, 500, 500, pathImage, randomImage);
                            Common.util.Function.ResizeImageNew(item, 1000, 1000, pathImage, randomImage);
                            productImage.ImgSource = strurlimage;
                            productImages.Add(productImage);
                        }
                    }
                }
                if (avatar != null && avatar.ContentLength > 0)
                {
                    var check = Util.CheckFileImage(avatar);
                    if (check == 0)
                    {
                        ViewBag.Mes = "File quá lớn";
                        return RedirectToAction("Index");
                    }
                    else if (check == 1)
                    {
                        ViewBag.Mes = "File không đúng định dạng";
                        return RedirectToAction("Index");
                    }
                    ProductImage productImage = new ProductImage();
                    // TourInfo entity=new TourInfo();
                    //Random rdImage = new Random();
                    string randomImage = Guid.NewGuid().ToString();
                    string fileNameImage = Common.util.Function.ConvertFileName(avatar.FileName);
                    string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                    var strurlimage = Common.util.Function.ResizeImageNew(avatar, 300, 300, pathImage, randomImage);
                    Common.util.Function.ResizeImageNew(avatar, 500, 500, pathImage, randomImage);
                    Common.util.Function.ResizeImageNew(avatar, 1000, 1000, pathImage, randomImage);
                    productImage.ImgSource = randomImage + "_" + fileNameImage;
                    productImage.IsAvatar = 1;
                    productImages.Add(productImage);
                }

                #endregion image

                objproduct.SeoKeyword = objproduct.Code;//Fix code
                objproduct.SeoTitle = objproduct.ProductName;//fix code
                objproduct.SeoDescription = objproduct.Description;//fix code
                objproduct.ProductProperties = productProperties;
                objproduct.ProductImages = productImages;
                _productsBusiness.AddNew(objproduct);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //Write log
                throw;
            }
        }

        #region Add

        [HttpGet]
        public ActionResult Add()
        {
            var loadDropdown = new LoadDropdown();
            var listDropdowCate = new List<LoadDropdown.DropdowCate>();
            //ViewBag.stateProduct = loadDropdown.SearchCategoryByName(ref listDropdowCate);
            ViewBag.statusProduct = _loadCombo.InitSelectListItemStatus();
            //Product obj = _productsBusiness.GetById(id);
            ViewBag.stateProducttwo = _loadCombo.InitSelectListItemState(1);
            ViewBag.Manufaceture = loadDropdown.LoadComBoManufaceture(0, 0);
            //  ViewBag.NewGroupID = loadDropdown.LoadNewGroup(0, 0);
            ViewBag.SliderID = loadDropdown.LoadListSlider(0, 0);
            ViewBag.PromotionListID = _loadCombo.InitSelectListPromotionList(null);
            //     string html = new BuyGroup365.Models.Common.Util().HtmlCate(-1, 0);
            ViewBag.htmlCate = _loadCombo.SearchCategoryByNamecb(ref listDropdowCate);
            Product obj = new Product();

            ViewBag.ColWidth = _loadCombo.InitSelectListItemColWidth();
            ViewBag.Tags = GetTag(0, 1, "");
            return View(obj);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(Product obj, long categoryproduct,
            long? manufaceture, long? SliderID, string avatar, string description, string LandingPage, string LongImages, string Promotion, long? PromotionListID, string[] file, string[] proprertyvalue, int State, string[] tags, HttpPostedFileBase[] filePrice, string[] catalogProducts, string ModifyDate, string IsNew, string IsAttractive, string IsVip, string IsProductBig, string IsHanded, string IsHome, long CatalogMain)
        {
            // try
            {
                using (var ts = new TransactionScope())
                {
                    Product entity = new Product();
                    //  entity.CatalogId = null;
                    entity.ManufacturerId = manufaceture;
                    entity.SliderID = SliderID;
                    entity.ProductProperties = obj.ProductProperties;
                    entity.ProductName = obj.ProductName;
                    entity.ProductNameShort = obj.ProductNameShort;
                    string friendly = "";
                    entity.NewGroupID = CatalogMain;       //Dùng trường danh mục tin để đặt danh mục sản phẩm chính
                    entity.ShowType = obj.ShowType;

                    entity.Promotion = Server.HtmlDecode(Promotion);
                    entity.Model = obj.Model;// Tên sản phẩm rút gọn
                    entity.Price = obj.Price;
                    entity.Summary = obj.Summary;
                    entity.Description = Server.HtmlDecode(description);
                    entity.LandingPage = Server.HtmlDecode(LandingPage);  // Thống số kỹ thuật rút gọn
                    entity.Cost = obj.Cost;
                    entity.Weight = obj.Weight;
                    entity.TypeOfCurrency = obj.TypeOfCurrency;
                    entity.SeoTitle = !string.IsNullOrEmpty(obj.SeoTitle) ? obj.SeoTitle : obj.ProductName;
                    entity.SeoKeyword = !string.IsNullOrEmpty(obj.SeoKeyword) ? obj.SeoKeyword : obj.ProductName;
                    entity.SeoDescription = !string.IsNullOrEmpty(obj.SeoDescription) ? obj.SeoDescription : obj.ProductName;
                    entity.ShareTitle = !string.IsNullOrEmpty(obj.ShareTitle) ? obj.ShareTitle : obj.ProductName;
                    entity.ShareKeyword = !string.IsNullOrEmpty(obj.ShareKeyword) ? obj.ShareKeyword : obj.ProductName;
                    entity.ShareDescription = !string.IsNullOrEmpty(obj.ShareDescription) ? obj.ShareDescription : obj.ProductName;
                    entity.State = obj.State; // Check lỗi này

                    //entity.Tags = obj.Tags;
                    entity.Status = obj.Status;
                    entity.PromotionID = PromotionListID;
                    if (!string.IsNullOrEmpty(IsAttractive) && IsAttractive == "1") entity.IsAttractive = true;
                    else entity.IsAttractive = false;
                    if (!string.IsNullOrEmpty(IsNew) && IsNew == "1") entity.IsNew = true;
                    else entity.IsNew = false;
                    if (!string.IsNullOrEmpty(IsVip) && IsVip == "1") entity.IsVip = true;
                    else entity.IsVip = false;
                    if (!string.IsNullOrEmpty(IsProductBig) && IsProductBig == "1") entity.IsProductBig = true;
                    else entity.IsProductBig = false;
                    if (!string.IsNullOrEmpty(IsHanded) && IsHanded == "1") entity.IsHanded = true;
                    else entity.IsHanded = false;
                    if (!string.IsNullOrEmpty(IsHome) && IsHome == "1") entity.IsHome = true;
                    else entity.IsHome = false;
                    entity.Code = obj.Code;
                    try
                    {
                        if (ModifyDate != null)
                            entity.ModifyDate = DateTime.ParseExact(ModifyDate.Replace('-', '/'), "dd/MM/yyyy", CultureInfo.CurrentCulture);
                        else
                            entity.ModifyDate = DateTime.Now;
                    }
                    catch { entity.ModifyDate = DateTime.Now; }
                    entity.ColWidth = obj.ColWidth;
                    entity.State = State;
                    //TODDO
                    // ADD CatalogProduct
                    List<CatalogProducts> catalogPros = entity.CatalogProducts.ToList();
                    string friendlyCata = "san-pham";
                    if (catalogProducts != null && catalogProducts.Count() > 0)
                    {
                        foreach (var catapro in catalogProducts)
                        {
                            var catalogProduct = new Common.CatalogProducts();

                            catalogProduct.CatalogId = long.Parse(catapro);
                            catalogPros.Add(catalogProduct);
                            friendlyCata = Function.ConvertFileName(_catalogsBusiness.GetById(catalogProduct.CatalogId).CatalogName);
                        }
                        if (CatalogMain != 0)
                            friendlyCata = Function.ConvertFileName(_catalogsBusiness.GetById(CatalogMain).CatalogName);
                    }

                    friendly = friendlyCata + "/" + Function.ConvertFileName(obj.ProductName) + duoilink;

                    entity.FriendlyUrl = friendly;
                    entity.CatalogProducts = catalogPros;

                    if (tags != null && tags.Count() > 0)
                    {
                        foreach (string tag in tags)
                        {
                            entity.Tags = entity.Tags + tag + ";";
                        }

                        entity.Tags = entity.Tags.Substring(0, entity.Tags.Count() - 1);
                    }

                    List<ProductImage> productImages = entity.ProductImages.ToList();
                    //Đôi tương dung đê bin lên form khi bị lỗi
                    var loadDropdown = new LoadDropdown();
                    var listDropdowCate = new List<LoadDropdown.DropdowCate>();
                    ViewBag.stateProduct = loadDropdown.SearchCategoryByName(ref listDropdowCate);

                    // obj = _productsBusiness.GetById(id);
                    ViewBag.stateProducttwo = _loadCombo.InitSelectListItemState(obj.State);
                    ViewBag.Manufaceture = loadDropdown.LoadComBoManufaceture(obj.CatalogId, obj.ManufacturerId);
                    //   ViewBag.NewGroupID = loadDropdown.LoadNewGroup(obj.CatalogId, obj.NewGroupID);
                    string html = new BuyGroup365.Models.Common.Util().HtmlCate(-1, obj.CatalogId);
                    ViewBag.htmlCate = html;
                    //kêt thúc

                    #region image

                    if (LongImages != "")
                        entity.LongImages = LongImages;
                    // var productImages = new List<ProductImage>();
                    if (file != null)
                    {
                        foreach (string item in file)
                        {
                            if (item != "")
                            {
                                var productImage = new ProductImage();

                                productImage.ImgSource = item;
                                productImages.Add(productImage);
                            }
                        }
                    }

                    var productImage1 = new ProductImage();
                    if (avatar != "")
                        productImage1.ImgSource = avatar;
                    productImage1.IsAvatar = 1;
                    productImages.Add(productImage1);

                    #endregion image

                    entity.ProductImages = productImages;

                    #region image Price

                    List<ProductImagesPrice> listImagesPrice = entity.ProductImagesPrices.ToList();

                    try
                    {
                        string[] PriceList = Request.Form.GetValues("ProductPrice");
                        string[] ProductNameSub = Request.Form.GetValues("ProductNameSub");
                        string[] NoteList = Request.Form.GetValues("ProductNote");
                        // var productImages = new List<ProductImage>();
                        if (PriceList != null)
                        {
                            for (int j = 0; j < PriceList.Count(); j++)
                            {
                                var productImagePrice = new ProductImagesPrice();
                                //if (filePrice[j] != null && filePrice[j].ContentLength > 0)
                                //{
                                //    var check = Util.CheckFileImage(filePrice[j]);
                                //    if (check == 0)
                                //    {
                                //        ViewBag.Mes = "File quá lớn";
                                //        return View();
                                //    }
                                //    else if (check == 1)
                                //    {
                                //        ViewBag.Mes = "File không đúng định dạng";
                                //        return View();
                                //    }

                                //    string randomImage = Guid.NewGuid().ToString();
                                //    string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                                //    string strurlimage = Function.ResizeImageNew(filePrice[j], 300, 300, pathImage, randomImage);
                                //    Function.ResizeImageNew(filePrice[j], 500, 500, pathImage, randomImage);
                                //    Function.ResizeImageNew(filePrice[j], 1000, 1000, pathImage, randomImage);
                                //    productImagePrice.ImageUrl = strurlimage;
                                //}
                                try
                                {
                                    productImagePrice.PriceName = ProductNameSub[j];
                                }
                                catch
                                {
                                    productImagePrice.PriceName = "";
                                }
                                try
                                {
                                    productImagePrice.Price = double.Parse(PriceList[j]);
                                }
                                catch
                                {
                                    productImagePrice.Price = 0;
                                }
                                productImagePrice.Note = NoteList[j];
                                if (productImagePrice.PriceName != null & productImagePrice.PriceName != "")
                                    listImagesPrice.Add(productImagePrice);
                            }
                        }

                        entity.ProductImagesPrices = listImagesPrice;
                    }
                    catch { }

                    #endregion image Price

                    #region Tin tuc ve sp

                    try
                    {
                        string[] NewAbout = Request.Form.GetValues("hiddenNewID");

                        string stringNewAbout = "";
                        if (NewAbout != null)
                        {
                            for (int j = 0; j < NewAbout.Count(); j++)
                            {
                                try
                                {
                                    stringNewAbout += NewAbout[j] + ";";
                                }
                                catch
                                {
                                }
                            }

                            entity.NewAbout = stringNewAbout;
                        }
                    }
                    catch { }

                    #endregion Tin tuc ve sp

                    #region Huong dan ve sp

                    try
                    {
                        string[] IntroSP = Request.Form.GetValues("hiddenNewIDHD");

                        string stringIntroSP = "";
                        if (IntroSP != null)
                        {
                            for (int j = 0; j < IntroSP.Count(); j++)
                            {
                                try
                                {
                                    stringIntroSP += IntroSP[j] + ";";
                                }
                                catch
                                {
                                }
                            }

                            entity.IntroProduct = stringIntroSP;
                        }
                    }
                    catch { }

                    #endregion Huong dan ve sp

                    #region SP liên quan

                    try
                    {
                        string[] RelativesProduct = Request.Form.GetValues("hiddenRelativesProductID");

                        string stringRelativesProduct = "";
                        if (RelativesProduct != null)
                        {
                            for (int j = 0; j < RelativesProduct.Count(); j++)
                            {
                                try
                                {
                                    stringRelativesProduct += RelativesProduct[j] + ";";
                                }
                                catch
                                {
                                }
                            }

                            entity.ProductRelative = stringRelativesProduct;
                        }
                    }
                    catch { }

                    #endregion SP liên quan

                    #region Property

                    var productProperties = new List<ProductProperty>();

                    if (proprertyvalue != null)
                    {
                        List<string> listobjproperty = proprertyvalue.ToList();
                        foreach (string item in listobjproperty)
                        {
                            string[] splitobj = item.Split('|');
                            CatalogProperty cateproperty = _catalogPropertiesBusiness.GetById(long.Parse(splitobj[0]));
                            for (int i = 1; i < splitobj.Count(); i++)
                            {
                                var valuepro = splitobj[i].Split('#');
                                var productProperty = new ProductProperty
                                {
                                    Name = cateproperty.Name,
                                    Key = cateproperty.Key,
                                    Value = valuepro[1],
                                    CatalogPropertyValueId = long.Parse(valuepro[0]),
                                    CatalogPropertyId = cateproperty.Id,
                                    ValueUrl = Function.ConvertFileName(valuepro[1])
                                };
                                productProperties.Add(productProperty);
                            }
                        }
                    }

                    entity.ProductProperties = productProperties;

                    #endregion Property

                    #region ATTRIBUTE

                    string[] keyList = Request.Form.GetValues("att_key");
                    string[] valueList = Request.Form.GetValues("att_value");
                    string[] TypeList = Request.Form.GetValues("TypePP");
                    List<ProductAttribute> listAtrt = new List<ProductAttribute>();
                    for (int i = 0; i < keyList.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(keyList[i]))
                        {
                            ProductAttribute item = new ProductAttribute();
                            item.Key = keyList[i];
                            item.Value = valueList[i];
                            item.TypePP = int.Parse(TypeList[i]);
                            listAtrt.Add(item);
                        }
                    }
                    entity.ProductAttributes = listAtrt;

                    #endregion ATTRIBUTE

                    long ProductId;
                    _productsBusiness.AddNew(entity);
                    ProductId = entity.Id;
                    try
                    {
                        Common.FriendlyUrl friendlyUrl = new Common.FriendlyUrl();
                        Random rnd = new Random();
                        int ngaunhien = rnd.Next(1, 100);
                        friendlyUrl.ItemId = ProductId;
                        friendlyUrl.Link = friendly;
                        friendlyUrl.ControllerName = "Home";
                        friendlyUrl.ActionName = "ProductDetail";
                        friendlyUrl.NameLink = entity.FriendlyUrl + ngaunhien.ToString();
                        friendlyUrl.NameSpaces = "BuyGroup365.Controllers";
                        friendlyUrl.Order = 0;

                        _friendlyUrlBusines.InsertLink(friendlyUrl);
                        RefreshFriendly.BindataSiteUrl();
                    }
                    catch { }

                    ts.Complete();
                    return RedirectToAction("Index");
                }
            }
            // catch (Exception ex)
            {
                //Write log
                //     throw;
            }
        }

        #endregion Add

        [HttpGet]
        public ActionResult Edit(long id)
        {
            try
            {
                var loadDropdown = new LoadDropdown();
                var listDropdowCate = new List<LoadDropdown.DropdowCate>();
                //ViewBag.stateProduct = loadDropdown.SearchCategoryByName(ref listDropdowCate);
                ViewBag.statusProduct = _loadCombo.InitSelectListItemStatus();

                Product obj = _productsBusiness.GetById(id);
                var ListCatalogPro = obj.CatalogProducts.ToList();
                ViewBag.stateProducttwo = _loadCombo.InitSelectListItemState(obj.State);
                ViewBag.Manufaceture = loadDropdown.LoadComBoManufaceture(obj.CatalogId, obj.ManufacturerId);

                // ViewBag.NewGroupID = loadDropdown.LoadNewGroup(0, obj.NewGroupID);  // Lấy danh sách nhóm tin
                ViewBag.SliderID = loadDropdown.LoadListSlider(0, obj.SliderID);
                string html = _loadCombo.SearchCategoryByNamecb(ref listDropdowCate, ListCatalogPro, obj.NewGroupID);
                ViewBag.htmlCate = html;
                ViewBag.PromotionListID = _loadCombo.InitSelectListPromotionList(obj.PromotionID);
                ViewBag.ColWidth = _loadCombo.InitSelectListItemColWidth(obj.ColWidth);
                if (obj.NewGroupID != null)
                    ViewBag.CatalogMain = obj.NewGroupID;
                else
                    ViewBag.CatalogMain = 0;
                ViewBag.Tags = GetTag(id, 1, "");
                //if (obj.ProductAttributes != null)
                //{
                //    string attHtm = "";
                //    foreach (ProductAttribute item in obj.ProductAttributes)
                //    {
                //        attHtm += "<div class=\"form-group\" id=\"item_" + item.Id + "\"><input type=\"text\" name=\"att_key\" value=\"" + item.Key + "\" /> <input type=\"text\" name=\"att_value\" value=\"" + item.Value + "\" />" +
                //                                                   "<a onclick=\"delAttribute(" + item.Id + ")\" >Xóa</a></div>";
                //    }
                //    ViewBag.Attribute = attHtm;
                //}

                return View(obj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult AddAttribute(long productId, string key, string value, int TypePP)
        {
            if (!string.IsNullOrEmpty(key))
            {
                ProductAttributesBusiness productAttributesBusiness = new ProductAttributesBusiness();
                ProductAttribute item = new ProductAttribute();
                item.ProductId = productId;
                item.Key = key;
                item.Value = value;
                item.TypePP = TypePP;
                productAttributesBusiness.AddNew(item);
                string select1 = "";
                string select2 = "";
                if (TypePP == 1)
                { select1 = "selected"; }
                if (TypePP == 2)
                { select2 = "selected"; }
                string html = "<div class=\"form-group\" id=\"item_" + item.Id + "\"><input class=\"key\" type=\"text\" name=\"att_key\" style=\"width: 200px\" value=\"" + item.Key + "\" /> <input class=\"value\" style=\"width: 500px\" type=\"text\" name=\"att_value\" value=\"" + item.Value + "\" />" +
                    "<select class=\"form-control\" name=\"TypePP\" style=\"float: left;width: 141px;margin-right: 10px; height: 30px;\">" +
                                                           " <option value = \"1\"" + select1 + ">Dưới khuyến mãi</option>" +
                                                            "<option value = \"2\"" + select2 + " > Bên trái</option>" +
                                                        "</select>" +
                                                                   "<a onclick=\"updateAttribute(" + item.Id + ")\" >Cập nhât</a> | <a onclick=\"delAttribute(" + item.Id + ")\" >Xóa</a></div>";
                return Json(html);
            }
            return Json(0);
        }

        [HttpPost]
        public JsonResult UpdateAttribute(long id, string key, string value, int TypePP)
        {
            if (!string.IsNullOrEmpty(key))
            {
                ProductAttributesBusiness productAttributesBusiness = new ProductAttributesBusiness();
                ProductAttribute item = productAttributesBusiness.GetById(id);

                item.Key = key;
                item.Value = value;
                item.TypePP = TypePP;
                productAttributesBusiness.Edit(item);

                return Json(1);
            }
            return Json(0);
        }

        [HttpPost]
        public JsonResult DelAttribute(long id)
        {
            if (id > 0)
            {
                ProductAttributesBusiness productAttributesBusiness = new ProductAttributesBusiness();
                productAttributesBusiness.Remove(id);
                return Json(1);
            }
            return Json(0);
        }

        [HttpGet]
        public JsonResult DeleteImage(long id)
        {
            if (id > 0)
            {
                ProductImagesBusiness productImagesBusiness = new ProductImagesBusiness();
                productImagesBusiness.Remove(id);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteImagePrice(long id)
        {
            if (id > 0)
            {
                ProductImagesPriceBusiness productImagesPriceBusiness = new ProductImagesPriceBusiness();
                productImagesPriceBusiness.Remove(id);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Product obj, long categoryproduct,
            long? manufaceture, long? SliderID, string avatar, string description, string LongImages, string LandingPage, string Promotion, long? PromotionListID, string[] file, string[] proprertyvalue, string[] tags, HttpPostedFileBase[] filePrice, string FriendLyHD, string[] catalogProducts, string ModifyDate, string IsNew, string IsAttractive, string IsVip, string IsProductBig, string IsHanded, string IsHome, long CatalogMain)
        {
            try
            {
                using (var ts = new TransactionScope())
                {
                    string friendly = "";
                    Product entity = _productsBusiness.GetById(obj.Id);
                    //  entity.CatalogId = categoryproduct;
                    entity.SliderID = SliderID;
                    entity.ProductProperties = obj.ProductProperties;
                    entity.ProductName = obj.ProductName;
                    entity.ProductNameShort = obj.ProductNameShort;
                    entity.ManufacturerId = manufaceture;
                    entity.ShowType = obj.ShowType;
                    entity.Model = obj.Model;    // Tên sản phẩm rút gọn
                    entity.Price = obj.Price;
                    entity.ColWidth = obj.ColWidth;
                    entity.Summary = obj.Summary;

                    entity.Promotion = Server.HtmlDecode(Promotion);
                    entity.Description = Server.HtmlDecode(description);
                    entity.LandingPage = Server.HtmlDecode(LandingPage);
                    entity.Cost = obj.Cost;
                    entity.Weight = obj.Weight;
                    entity.TypeOfCurrency = obj.TypeOfCurrency;
                    entity.SeoTitle = !string.IsNullOrEmpty(obj.SeoTitle) ? obj.SeoTitle : obj.ProductName;
                    entity.SeoKeyword = !string.IsNullOrEmpty(obj.SeoKeyword) ? obj.SeoKeyword : obj.ProductName;
                    entity.SeoDescription = !string.IsNullOrEmpty(obj.SeoDescription) ? obj.SeoDescription : obj.ProductName;
                    entity.ShareTitle = !string.IsNullOrEmpty(obj.ShareTitle) ? obj.ShareTitle : obj.ProductName;
                    entity.ShareKeyword = !string.IsNullOrEmpty(obj.ShareKeyword) ? obj.ShareKeyword : obj.ProductName;
                    entity.ShareDescription = !string.IsNullOrEmpty(obj.ShareDescription) ? obj.ShareDescription : obj.ProductName;
                    entity.State = obj.State; // Check lỗi này
                    entity.Status = obj.Status;
                    entity.PromotionID = PromotionListID;
                    if (!string.IsNullOrEmpty(IsAttractive) && IsAttractive == "1") entity.IsAttractive = true;
                    else entity.IsAttractive = false;
                    if (!string.IsNullOrEmpty(IsNew) && IsNew == "1") entity.IsNew = true;
                    else entity.IsNew = false;
                    if (!string.IsNullOrEmpty(IsVip) && IsVip == "1") entity.IsVip = true;
                    else entity.IsVip = false;
                    if (!string.IsNullOrEmpty(IsProductBig) && IsProductBig == "1") entity.IsProductBig = true;
                    else entity.IsProductBig = false;
                    if (!string.IsNullOrEmpty(IsHanded) && IsHanded == "1") entity.IsHanded = true;
                    else entity.IsHanded = false;
                    if (!string.IsNullOrEmpty(IsHome) && IsHome == "1") entity.IsHome = true;
                    else entity.IsHome = false;

                    //   entity.Tags = obj.Tags;
                    entity.NewGroupID = CatalogMain;       //Dùng trường danh mục tin để đặt danh mục sản phẩm chính
                    entity.Code = obj.Code;
                    //entity.CongNgheSanXuat = obj.CongNgheSanXuat;
                    //entity.XuatXu = obj.XuatXu;
                    //entity.NhapKhauLapRap = obj.NhapKhauLapRap;
                    //entity.ChatLieu = obj.ChatLieu;
                    //entity.MauSac = obj.MauSac;
                    //entity.KichCo = obj.KichCo;
                    //entity.DoBen = obj.DoBen;
                    //entity.BoBaoGom = obj.BoBaoGom;
                    //entity.BaoHanh = obj.BaoHanh;
                    //TODDO
                    try
                    {
                        if (ModifyDate != null)
                            entity.ModifyDate = DateTime.ParseExact(ModifyDate.Replace('-', '/'), "dd/MM/yyyy", CultureInfo.CurrentCulture);
                        else
                            entity.ModifyDate = DateTime.Now;
                    }
                    catch
                    {
                        entity.ModifyDate = DateTime.Now;
                    }

                    entity.Tags = string.Empty;
                    if (tags != null && tags.Count() > 0)
                    {
                        foreach (string tag in tags)
                        {
                            entity.Tags = entity.Tags + tag + ";";
                        }

                        entity.Tags = entity.Tags.Substring(0, entity.Tags.Count() - 1);
                    }
                    _catalogProduct.RemoveList(obj.Id);
                    List<CatalogProducts> catalogPros = entity.CatalogProducts.ToList();
                    string friendlyCata = "san-pham";
                    if (catalogProducts != null && catalogProducts.Count() > 0)
                    {
                        foreach (var catapro in catalogProducts)
                        {
                            var catalogProduct = new Common.CatalogProducts();

                            catalogProduct.CatalogId = long.Parse(catapro);
                            catalogProduct.ProductId = obj.Id;
                            catalogPros.Add(catalogProduct);
                            friendlyCata = Function.ConvertFileName(_catalogsBusiness.GetById(catalogProduct.CatalogId).CatalogName);
                        }
                        if (CatalogMain != 0)
                            friendlyCata = Function.ConvertFileName(_catalogsBusiness.GetById(CatalogMain).CatalogName);
                    }
                    friendly = friendlyCata + "/" + Function.ConvertFileName(obj.ProductName) + duoilink;

                    entity.FriendlyUrl = friendly;

                    entity.CatalogProducts = catalogPros;

                    List<ProductImage> productImages = entity.ProductImages.ToList();
                    //Đôi tương dung đê bin lên form khi bị lỗi
                    var loadDropdown = new LoadDropdown();
                    var listDropdowCate = new List<LoadDropdown.DropdowCate>();
                    ViewBag.stateProduct = loadDropdown.SearchCategoryByName(ref listDropdowCate);

                    // obj = _productsBusiness.GetById(id);
                    ViewBag.stateProducttwo = _loadCombo.InitSelectListItemState(obj.State);
                    ViewBag.Manufaceture = loadDropdown.LoadComBoManufaceture(obj.CatalogId, obj.ManufacturerId);
                    //  ViewBag.NewGroupID = loadDropdown.LoadNewGroup(obj.CatalogId, obj.NewGroupID);
                    string html = new BuyGroup365.Models.Common.Util().HtmlCate(-1, obj.CatalogId);
                    ViewBag.htmlCate = html;
                    if (LongImages != "")
                    {
                        entity.LongImages = LongImages;
                    }
                    //kêt thúc
                    if (avatar != "")
                    {
                        ProductImage productImage = entity.ProductImages.First(x => x.IsAvatar == 1);
                        productImage.ImgSource = avatar;
                        productImages.Add(productImage);
                    }
                    if (file != null)
                    {
                        foreach (string item in file)
                        {
                            if (item != "")
                            {
                                var productImage1 = new ProductImage();
                                productImage1.ImgSource = item;
                                productImages.Add(productImage1);
                            }
                        }
                    }
                    entity.ProductImages = productImages;

                    #region image Price

                    List<ProductImagesPrice> listImagesPrice = entity.ProductImagesPrices.ToList();

                    try
                    {
                        string[] PriceList = Request.Form.GetValues("ProductPrice");
                        string[] ProductNameSub = Request.Form.GetValues("ProductNameSub");
                        string[] NoteList = Request.Form.GetValues("ProductNote");
                        // var productImages = new List<ProductImage>();
                        if (PriceList != null)
                        {
                            for (int j = 0; j < PriceList.Count(); j++)
                            {
                                var productImagePrice = new ProductImagesPrice();
                                //if (filePrice[j] != null && filePrice[j].ContentLength > 0)
                                //{
                                //    var check = Util.CheckFileImage(filePrice[j]);
                                //    if (check == 0)
                                //    {
                                //        ViewBag.Mes = "File quá lớn";
                                //        return View();
                                //    }
                                //    else if (check == 1)
                                //    {
                                //        ViewBag.Mes = "File không đúng định dạng";
                                //        return View();
                                //    }

                                //    string randomImage = Guid.NewGuid().ToString();
                                //    string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                                //    string strurlimage = Function.ResizeImageNew(filePrice[j], 300, 300, pathImage, randomImage);
                                //    Function.ResizeImageNew(filePrice[j], 500, 500, pathImage, randomImage);
                                //    Function.ResizeImageNew(filePrice[j], 1000, 1000, pathImage, randomImage);
                                //    productImagePrice.ImageUrl = strurlimage;
                                //}
                                try
                                {
                                    productImagePrice.PriceName = ProductNameSub[j];
                                }
                                catch
                                {
                                    productImagePrice.PriceName = "";
                                }
                                try
                                {
                                    productImagePrice.Price = double.Parse(PriceList[j]);
                                }
                                catch
                                {
                                    productImagePrice.Price = 0;
                                }
                                productImagePrice.Note = NoteList[j];
                                if (ProductNameSub[j] != null && ProductNameSub[j] != "")
                                    listImagesPrice.Add(productImagePrice);
                            }
                        }

                        entity.ProductImagesPrices = listImagesPrice;
                    }
                    catch { }

                    #endregion image Price

                    #region Tin tuc ve sp

                    try
                    {
                        string[] NewAbout = Request.Form.GetValues("hiddenNewID");

                        string stringNewAbout = "";
                        if (NewAbout != null)
                        {
                            for (int j = 0; j < NewAbout.Count(); j++)
                            {
                                try
                                {
                                    stringNewAbout += NewAbout[j] + ";";
                                }
                                catch
                                {
                                }
                            }

                            entity.NewAbout = stringNewAbout;
                        }
                    }
                    catch { }

                    #endregion Tin tuc ve sp

                    #region Huong dan ve sp

                    try
                    {
                        string[] IntroSP = Request.Form.GetValues("hiddenNewIDHD");

                        string stringIntroSP = "";
                        if (IntroSP != null)
                        {
                            for (int j = 0; j < IntroSP.Count(); j++)
                            {
                                try
                                {
                                    stringIntroSP += IntroSP[j] + ";";
                                }
                                catch
                                {
                                }
                            }

                            entity.IntroProduct = stringIntroSP;
                        }
                    }
                    catch { }

                    #endregion Huong dan ve sp

                    #region SP liên quan

                    try
                    {
                        string[] RelativesProduct = Request.Form.GetValues("hiddenRelativesProductID");

                        string stringRelativesProduct = "";
                        if (RelativesProduct != null)
                        {
                            for (int j = 0; j < RelativesProduct.Count(); j++)
                            {
                                try
                                {
                                    stringRelativesProduct += RelativesProduct[j] + ";";
                                }
                                catch
                                {
                                }
                            }

                            entity.ProductRelative = stringRelativesProduct;
                        }
                    }
                    catch { }

                    #endregion SP liên quan

                    #region Property

                    ICollection<ProductProperty> listproduc = entity.ProductProperties;
                    foreach (ProductProperty productProperty in listproduc)
                    {
                        _productPropertiesBusiness.Remove(productProperty.Id);
                    }
                    var productProperties = new List<ProductProperty>();

                    if (proprertyvalue != null)
                    {
                        List<string> listobjproperty = proprertyvalue.ToList();
                        foreach (string item in listobjproperty)
                        {
                            string[] splitobj = item.Split('|');
                            CatalogProperty cateproperty = _catalogPropertiesBusiness.GetById(long.Parse(splitobj[0]));
                            for (int i = 1; i < splitobj.Count(); i++)
                            {
                                var valuepro = splitobj[i].Split('#');
                                var productProperty = new ProductProperty
                                {
                                    Name = cateproperty.Name,
                                    Key = cateproperty.Key,
                                    Value = valuepro[1],
                                    CatalogPropertyValueId = long.Parse(valuepro[0]),
                                    ProductId = entity.Id,
                                    CatalogPropertyId = cateproperty.Id,
                                    ValueUrl = Function.ConvertFileName(valuepro[1])
                                };
                                productProperties.Add(productProperty);
                            }
                        }
                    }

                    #endregion Property

                    _productPropertiesBusiness.AddRange(productProperties);
                    _productsBusiness.Edit(entity);

                    try
                    {
                        Common.FriendlyUrl friendlyUrl = new Common.FriendlyUrl();
                        Random rnd = new Random();
                        int ngaunhien = rnd.Next(1, 100);
                        string linkcu = "";

                        if (FriendLyHD != friendly)
                        {
                            linkcu = FriendLyHD;
                        }
                        else
                        {
                            linkcu = "";
                        }
                        friendlyUrl.ItemId = obj.Id;
                        friendlyUrl.Link = friendly;
                        friendlyUrl.ControllerName = "Home";
                        friendlyUrl.ActionName = "ProductDetail";
                        friendlyUrl.NameLink = friendly + ngaunhien.ToString();
                        friendlyUrl.NameSpaces = "BuyGroup365.Controllers";
                        friendlyUrl.Order = 0;

                        _friendlyUrlBusines.UpdateLink(linkcu, friendlyUrl);
                        RefreshFriendly.BindataSiteUrl();
                    }
                    catch { }

                    ts.Complete();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                //Write log
                throw;
            }
        }

        public JsonResult ApprovedProduct(long productid, int status)
        {
            try
            {
                var product = _productsBusiness.GetById(productid);
                product.Status = status;
                _productsBusiness.Edit(product);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(long id)
        {
            try
            {
                _productsBusiness.Remove(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        /// <summary>
        /// Hàm gọi json đê bind dữ liêu vào dropdown
        /// </summary>
        /// <param name="cate"></param>
        /// <returns></returns>
        public JsonResult GetManufaceByCategory(long cate)
        {
            try
            {
                ObjGetPropertyAndManuByCate objGetPropertyAndManuByCate = new ObjGetPropertyAndManuByCate();
                /*var htm = "";
                var listobj = MemberController.ListCatalogsManufacturerses.Where(x => x.CatalogId == cate).ToList();
                if (listobj.Any())
                {
                    foreach (var item in listobj)
                    {
                        htm += " <option value=\"" + item.ManufacturerId + "\" onclick=\"GetManuface("+item.ManufacturerId+")\">" + item.Manufacturer.ManufacturerName +
                               "</option>";
                    }

                    //return Json(htm);
                }
                else
                {
                    htm += " <option value=\"" + -1 + "\" onclick=\"GetManuface(" + -1+ ")\">Not found Manuface</option>";
                }
                objGetPropertyAndManuByCate.HtmlManuface = htm;*/
                var listproperty = MemberController.ListCatalogProperties.Where(x => x.CatalogId == cate).ToList();
                var listjJsonProperties = new List<JsonProperty>();
                if (listproperty.Any())
                {
                    foreach (var item in listproperty)
                    {
                        JsonProperty entity = new JsonProperty();
                        entity.Id = item.Id;
                        entity.Name = item.Name;
                        entity.HtmlType = HtmlTypeProperty(item.Id);
                        entity.HtmlInputHidden = "<input  type=\"hidden\" id=\"proprerty" + item.Id + "\" name=\"proprertyvalue\" value=\"" + item.Id + "\"/>";
                        listjJsonProperties.Add(entity);
                    }
                    objGetPropertyAndManuByCate.JsonProperties = listjJsonProperties;
                }
                else
                {
                    objGetPropertyAndManuByCate.JsonProperties = listjJsonProperties;
                }
                return Json(objGetPropertyAndManuByCate);
            }
            catch (Exception)
            {
                return Json(0);// Lỗi rồi e

                //Write log
                throw;
            }
        }

        [HttpPost]
        public string GetTag(long id, int type, string key)
        {
            try
            {
                TagsBusiness tagsBusiness = new Business.ClassBusiness.TagsBusiness();
                var obj = tagsBusiness.GetList(type, key);

                var htmlcate = "";
                if (id == 0)
                {
                    foreach (var item in obj)
                    {
                        htmlcate += "<label class=\"checkbox-inline\"><input value=\"" + item.Id + "\" type=\"checkbox\" name=\"tags\">" + item.TagName + "</label>";
                    }
                }
                else
                {
                    string tags = _productsBusiness.GetById(id).Tags;

                    if (!string.IsNullOrEmpty(tags))
                    {
                        string[] arrayTag = tags.Split(';');
                        bool isCheck = false;

                        foreach (var item in obj)
                        {
                            isCheck = false;
                            foreach (var tag in arrayTag)
                            {
                                if (tag == item.Id.ToString()) { isCheck = true; break; }
                            }
                            if (isCheck)
                            {
                                htmlcate += "<label class=\"checkbox-inline\"><input value=\"" + item.Id + "\" type=\"checkbox\" name=\"tags\" checked>" + item.TagName + "</label>";
                            }
                            else
                            {
                                htmlcate += "<label class=\"checkbox-inline\"><input value=\"" + item.Id + "\" type=\"checkbox\" name=\"tags\">" + item.TagName + "</label>";
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in obj)
                        {
                            htmlcate += "<label class=\"checkbox-inline\"><input value=\"" + item.Id + "\" type=\"checkbox\" name=\"tags\">" + item.TagName + "</label>";
                        }
                    }
                }

                return htmlcate;
            }
            catch (Exception)
            {
                return "";
                //write log
                throw;
            }
        }

        public JsonResult GetCategory(long productId)
        {
            try
            {
                var obj = _productsBusiness.GetById(productId);
                HtmlDropdown html = new HtmlDropdown();
                var listcate = _loadCombo.InitDropCategorys(1);
                var htmlcate = "";
                var htmlmanu = "";
                foreach (var item in listcate)
                {
                    if (item.Value == obj.CatalogId.ToString())
                    {
                        htmlcate += "<option value='" + item.Value + "' selected='selected'>" + item.Text + "</option>";
                    }
                    else
                    {
                        htmlcate += "<option value='" + item.Value + "' >" + item.Text + "</option>";
                    }
                }
                var objectlist =
                    _catalogsManufacturersBusiness.GetDynamicQuery().Where(x => x.CatalogId == obj.CatalogId);
                foreach (var item in objectlist)
                {
                    if (item.ManufacturerId == obj.ManufacturerId)
                    {
                        htmlmanu += "<option value='" + item.ManufacturerId + "' selected='selected'>" + item.Manufacturer.ManufacturerName + "</option>";
                    }
                    else
                    {
                        htmlmanu += "<option value='" + item.ManufacturerId + "' >" + item.Manufacturer.ManufacturerName + "</option>";
                    }
                }
                html.HtmlCategory = htmlcate;
                html.HtmlManuface = htmlmanu;
                return Json(html);
            }
            catch (Exception)
            {
                return Json(0);
                //write log
                throw;
            }
        }

        public string HtmlTypeProperty(long proId)
        {
            var htm = "";
            try
            {
                var listObj = MemberController.ListCatalogPropertiesValues.Where(x => x.CatalogPropertieId == proId).ToList();
                if (listObj.Any())
                {
                    var objproperty = _catalogPropertiesBusiness.GetById(proId);

                    if (objproperty.Type == 1)
                    {
                        foreach (var item in listObj)
                        {
                            htm += "<label class='radio-inline'> <input onclick='SetvaluProperty(" + item.Id + "," + proId + ")' type='radio' name='optionsRadiosInline" + proId + "' id='optionsRadiosInline" + item.Id + "'  value='" + item.Value + "' >" + item.Value + "</label>";
                        }
                    }
                    else
                    {
                        foreach (var item in listObj)
                        {
                            htm += "<label class='checkbox-inline'>   <input  value='" + item.Value + "' onclick='SetvaluProperty(" + item.Id + "," + proId + ")'  type='checkbox'  id='optionsRadiosInline" + item.Id + "'>" + item.Value + "</label>";
                        }

                        //multiple
                    }
                    return htm;
                }
                else
                {
                    return "Not results value property!";
                }
            }
            catch (Exception)
            {
                //write log
                return "Not results value property!";
            }
        }

        public JsonResult GetPropertyValueByProId(long proId)
        {
            try
            {
                var listObj = _catalogPropertiesValueBusiness.GetDynamicQuery()
                    .Where(x => x.CatalogPropertieId == proId).ToList();
                if (listObj.Any())
                {
                    var objproperty = _catalogPropertiesBusiness.GetById(proId);
                    var htm = "";
                    if (objproperty.Type == 1)
                    {
                        foreach (var item in listObj)
                        {
                            htm += "<label class='radio-inline'> <input onclick='SetvaluProperty(" + item.Id + ")' type='radio' name='optionsRadiosInline' id='optionsRadiosInline" + item.Id + "'  value='" + item.Id + "' >" + item.Value + "</label>";
                        }
                    }
                    else
                    {
                        foreach (var item in listObj)
                        {
                            htm += "<label class='checkbox-inline'>   <input  value='" + item.Id + "'  type='checkbox'  id='optionsRadiosInline" + item.Id + "'>" + item.Value + "</label>";
                        }

                        //multiple
                    }
                    return Json(htm);
                }
                else
                {
                    return Json("Not results value property!");
                }
            }
            catch (Exception)
            {
                //write log
                return Json(0);
                throw;
            }
        }

        public class HtmlDropdown
        {
            public string HtmlCategory { get; set; }
            public string HtmlManuface { get; set; }
        }

        public class ObjGetPropertyAndManuByCate
        {
            public string HtmlManuface { get; set; }
            public List<JsonProperty> JsonProperties { get; set; }
        }

        public class JsonProperty
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string HtmlType { get; set; }
            public string HtmlInputHidden { get; set; }
        }

        public string HtmlCate(long prentid)
        {
            var i = 0;
            var j = 0;
            var html = "<ul>";
            //     var listobj = _catalogsBusiness.GetDynamicQuery().Where(x => x.ParentId == prentid).ToList();
            var listobj = CategoryProductController.ListCate.Where(x => x.ParentId == prentid).ToList();

            foreach (var item in listobj)
            {
                html += "<li><input type=\"checkbox\" id=\"item-" + i + "\"   onclick=\"GetValueManuface(" + item.Id + ")\" /><label for=\"item-" + i + "\"  onclick=\"GetValueManuface(" + item.Id + ")\" >" + item.CatalogName + "</label>";

                i++;
                var str = HtmlbyCate(item.Id, ref i);
                html += str;

                html += "</li>";
            }
            html += "</ul>";
            return html;
        }

        public string HtmlbyCate(long cateId, ref int i)
        {
            //  var i = 1;
            var j = 0;
            var html = "";
            html = "<ul>";
            //var listobj = _catalogsBusiness.GetDynamicQuery().Where(x => x.ParentId == cateId).ToList();
            var listobj = CategoryProductController.ListCate.Where(x => x.ParentId == cateId).ToList();
            if (listobj.Any())
            {
                foreach (var item in listobj)
                {
                    html += "<li><input type=\"checkbox\" id=\"item-" + i + "\" onclick=\"GetValueManuface(" + item.Id + ")\" /><label for=\"item-" + i + "\"   onclick=\"GetValueManuface(" + item.Id + ")\" >" + item.CatalogName + "</label>";
                    i++;
                    var str = HtmlbyCate(item.Id, ref i);
                    html += str;
                    html += "</li>";
                }
            }

            html += "</ul>";
            return html;
        }

        public JsonResult GetCatetagoryByParent(long cate)
        {
            try
            {
                HtmlDropdown htmlDropdown = new HtmlDropdown();
                var htm = "";
                var manuface = "";
                var listobj = _catalogsBusiness.GetDynamicQuery().Where(x => x.ParentId == cate).ToList();
                if (listobj.Any())
                {
                    htm +=
                        "<div class=\"box-oveflow\" id=\"box" + cate + "\"><ul class=\"list-category-suggestions\" >";
                    foreach (var catalog in listobj)
                    {
                        htm += "<li><a href=\"javascript:;\"  onclick=\"GetValueParent(" + catalog.Id + ")\">" +
                               catalog.CatalogName + "</a></li>";
                    }
                    htm += "</ul></div>";
                }
                else
                {
                    var listmanuface =
                        _catalogsManufacturersBusiness.GetDynamicQuery().Where(x => x.CatalogId == cate).ToList();
                    if (listmanuface.Any())
                    {
                        manuface +=
                            "<div class=\"box-oveflow\"><ul class=\"list-category-suggestions\" >";
                        foreach (var manu in listmanuface)
                        {
                            manuface += "<li><a href=\"javascript:;\"  onclick=\"GetManuface(" + manu.ManufacturerId + ")\">" +
                                   manu.Manufacturer.ManufacturerName + "</a></li>";
                        }
                        manuface += "</ul></div>";
                    }
                    else
                    {
                        manuface +=
                           "<div class=\"box-oveflow\"><ul class=\"list-category-suggestions\" >";
                        manuface += "<li><a href=\"javascript:;\"  onclick=\"GetManuface(0)\">" +
                                   "Not results</a></li>";

                        manuface += "</ul></div>";
                    }
                }
                htmlDropdown.HtmlCategory = htm;
                htmlDropdown.HtmlManuface = manuface;
                return Json(htmlDropdown);
            }
            catch (Exception)
            {
                //write log
                return Json(0);
                throw;
            }
        }

        #region Comment

        public ActionResult Comment(string key, int? page)
        {
            try
            {
                ViewData["status"] = true;
                ViewData["key"] = key;

                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = _commentsBusiness.GetListComments(key).ToPagedList(currentPageIndex, 20);

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

        public JsonResult PreviewCommnet(long productid)
        {
            try
            {
                var listCommebt = _commentsBusiness.GetDynamicQuery().OrderByDescending(x => x.CreateDate).Where(x => x.ProductId == productid && x.ParentId == null).ToList();

                //  ViewBag["ProductCommentID"] = productid;
                string body = ControllerExtensions.RenderRazorViewToString(this, "ContentComment", listCommebt);
                return Json(body, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public JsonResult DeleteComent(long id)
        {
            try
            {
                _commentsBusiness.Remove(id);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ContentComment()
        {
            return View();
        }

        public JsonResult ApprovedComment(long commentid, int status)
        {
            try
            {
                var Comments = _commentsBusiness.GetById(commentid);
                Comments.Status = status;
                _commentsBusiness.Edit(Comments);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ReplyComment(long productid, long commentid, int status, string TraLoicontent)
        {
            try
            {
                CommentsBusiness commentbusiness = new CommentsBusiness();
                Common.Comment comment = new Common.Comment();
                comment.Content = TraLoicontent;
                comment.CreateDate = DateTime.Now;
                comment.ProductId = productid;
                comment.ParentId = commentid;
                comment.Status = 2;
                comment.NickName = SessionUtility.GetSessionName(Session);
                comment.Phone = "";
                comment.Email = "";
                comment.Rate = 5;
                commentbusiness.AddNew(comment);
                return Json(comment.Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Comment
    }
}