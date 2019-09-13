using Business.ClassBusiness;
using BuyGroup365.Models.Home;
using Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;

namespace BuyGroup365.Controllers
{
    public class PartialController : Controller
    {
        //
        // GET: /Home/
        private static string Database_Name = ConfigurationSettings.AppSettings.Get("database_name");

        private CatalogsBusiness catalogBusiness = new CatalogsBusiness();
        private ProductsBusiness productBussiness = new ProductsBusiness();
        private NewsGroupBusiness newgroupBussiness = new NewsGroupBusiness();
        private long CatalogId1, CatalogId2, CatalogId3, CatalogId4;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCategory(string type, string info)
        {
            long id = long.Parse(info);
            if (id > 0)
            {
                Catalog temp = catalogBusiness.GetById(id);
                string CatalogName;
                if (temp.ParentId != -1)
                {
                    Catalog temp1 = catalogBusiness.GetById(temp.ParentId);

                    if (temp1.ParentId != -1)
                    {
                        Catalog temp2 = catalogBusiness.GetById(temp1.ParentId);

                        if (temp2.ParentId != -1)
                        {
                            Catalog temp3 = catalogBusiness.GetById(temp2.ParentId);
                            CatalogId1 = temp3.Id;
                            CatalogId2 = temp2.Id;
                            CatalogId3 = temp1.Id;
                            CatalogId4 = temp.Id;
                            CatalogName = temp3.CatalogName;
                        }
                        else
                        {
                            CatalogId1 = temp2.Id;
                            CatalogId2 = temp1.Id;
                            CatalogId3 = temp.Id;
                            CatalogName = temp2.CatalogName;
                        }
                    }
                    else
                    {
                        CatalogId1 = temp1.Id;
                        CatalogId2 = temp.Id;
                        CatalogName = temp1.CatalogName;
                    }
                }
                else
                {
                    CatalogId1 = temp.Id;
                    CatalogName = temp.CatalogName;
                }

                ViewData["CatalogName"] = CatalogName;

                string html = InitMenuCatalog(CatalogId1, 1);

                ViewData["Item"] = html;
            }
            else
            {
                ViewData["CatalogName"] = "Danh mục";

                string html = InitMenuCatalog(-1, 1);

                ViewData["Item"] = html;
            }
            return PartialView();
        }

        public ActionResult GetFilter(string type, string info, long id)
        {
            string mid = "";
            double priceForm = 0, priceTo = 0;
            string catalogUrl = "";
            string urlParam = "";
            string k = "";
            try
            {
                urlParam = Request.RawUrl.Split('?')[1];
                string[] listInfo = info.Split(';');
                // id = long.Parse(listInfo[0]);
                //mid = long.Parse(listInfo[1]);

                //priceForm = double.Parse(listInfo[2].Split(':')[0]);
                //priceTo = double.Parse(listInfo[2].Split(':')[1]);

                // catalogUrl = listInfo[3];

                catalogUrl = Request.RawUrl.Split('?')[0];
                // urlParam = listInfo[4];
                //    k = listInfo[5];
            }
            catch
            {
                catalogUrl = Request.RawUrl.Split('?')[0];
            }
            string[] arrayParam = urlParam.Split('&');
            string pramSql = "";
            foreach (string item in arrayParam)
            {
                if (!string.IsNullOrEmpty(item))
                    pramSql += string.Format("	AND (ProductProperties.[Key]='{0}' AND ProductProperties.ValueUrl IN ('{1}')) ", item.Split('=')[0], item.Split('=')[1].Replace(",", "','"));
            }

            string strQuery = BuildQueryFilterByManufacturer(id, mid, priceForm, priceTo, pramSql, k);

            BuyGroup365Entities entitis = new BuyGroup365Entities();
            var result = entitis.Database.SqlQuery<ManuItem>(strQuery).ToList();

            string paramsString = "";

            if (!string.IsNullOrEmpty(urlParam))
            {
                paramsString += "" + urlParam;
            }
            else
            {
                paramsString += catalogUrl;
            }

            ViewData["ItemMenuManufacturer"] = InitManufaturer(urlParam, catalogUrl, result);
            ViewData["ItemMenuPrice"] = InitFilterPrice(catalogUrl, id, mid, priceForm, priceTo, paramsString);
            ViewData["ItemMenuAttribute"] = InitFilterAttribute(catalogUrl, id, mid, priceForm, priceTo, pramSql, catalogUrl, k);

            return PartialView();
        }

        protected string BuildQueryFilterByManufacturer(long? id, string mid, double? priceFrom, double? priceTo, string param, string k)
        {
            string query = "", buildQuery = "";

            if (id != 0)
            {
                query = @"BEGIN
				            WITH temp(id)
			                as (

					                Select Id
					                From Catalogs
					                Where Id ={0}
					                Union All
					                Select b.Id
					                From temp as a, Catalogs as b
					                Where a.Id = b.ParentId

			                )

				                 SELECT ManufacturerId, Manufacturers.ManufacturerName,Manufacturers.Icon, COUNT(Manufacturers.Id) AS Total FROM( select distinct Products.Id, ManufacturerId  from
	                          temp join CatalogProducts ON temp.Id=CatalogProducts.CatalogId join  Products ON CatalogProducts.ProductId = Products.Id
	                            LEFT JOIN ProductProperties ON Products.Id=ProductProperties.ProductId

                                ) as A JOIN Manufacturers on ManufacturerId = Manufacturers.Id

                                       GROUP BY ManufacturerId, ManufacturerName,Manufacturers.Icon

		                END";
                //if (order == 1)//Mới nhất

                buildQuery = string.Format(query, id, mid, priceFrom, priceTo, param, k);
            }
            else
            {
                query = @"BEGIN

				                 SELECT Id, ManufacturerName, COUNT(Id) AS Total FROM Manufacturers

		                END";
                //if (order == 1)//Mới nhất

                buildQuery = string.Format(query, id, mid, priceFrom, priceTo, param, k);
            }

            return buildQuery;
        }

        protected string InitManufaturer(string urlParam, string catUrl, List<ManuItem> result)
        {
            string html = "";
            string id = "";
            int countMid = 0;
            NameValueCollection n = Request.QueryString;
            string check = "";
            bool checkbool = false;
            foreach (ManuItem re in result)
            {
                if (catUrl.Contains(re.ManufacturerName) || Request.RawUrl.Contains(re.ManufacturerId.ToString()))
                {
                    check = "class=\"check\"";
                    checkbool = true;
                }
            }
            for (int i = 0; i < n.Count; i++)
            {
                string k1 = n.GetKey(i);
                string v = n.Get(i);
                if (k1 == "mid")
                {
                    id = v;
                    countMid = v.Split(',').Count();
                }
            }
            //if (Request.Browser.IsMobileDevice)
            //{
            //    //Bản mobile
            //    html += "<div class=\"btn-group dropup\"><button type=\"button\" class=\"btn btn-secondary dropdown-toggle\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">  Hãng    </button> <div class=\"dropdown-menu\">";
            //    html += "<dl class=\"narrow-by\" id=\"_Hang\"><dt class=\"odd\">Hãng</dt> <dt class=\"toggle-tab mobile last even\" style=\"display: block\"></dt><dd class=\"Size toggle-content odd\"><ol class=\"\">";

            //    foreach (ManuItem re in result)
            //    {
            //        if (checkbool)
            //        {
            //            if (id.Contains(re.ManufacturerId.ToString()))
            //            {
            //                html += "<li><a " + check + " href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, -1, "mid", re.ManufacturerId.ToString()) + "\"><i class=\"iconcate-checkbox\"></i> <img width=\"70px\" height=\"30px\" src=\"" + re.Icon.Split('?')[0] + "medium/" + re.Icon.Split('?')[1] + "\"/></a></li>";
            //                ViewData["choosedfilter"] += "<a  href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, -1, "mid", re.ManufacturerId.ToString()) + "\" ><span>" + re.ManufacturerName + "</span><i class=\"iconcate-clearfil\"></i></a>";
            //            }
            //            else
            //            {
            //                html += "<li><a href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, 1, "mid", re.ManufacturerId.ToString()) + "\"><i class=\"iconcate-checkbox\"></i>  <img width=\"70px\" height=\"30px\" src=\"" + re.Icon.Split('?')[0] + "medium/" + re.Icon.Split('?')[1] + "\"/></a></li>";
            //            }
            //        }
            //        else
            //        {
            //            html += "<li><a href=\"" + Common.util.Function.InitUrlFilter(catUrl + "-" + re.ManufacturerName, n, 1, "mid", re.ManufacturerId.ToString()) + "\"><i class=\"iconcate-checkbox\"></i>  <img width=\"70px\" height=\"30px\" src=\"" + re.Icon.Split('?')[0] + "medium/" + re.Icon.Split('?')[1] + "\"/></a></li>";
            //        }
            //    }

            //    html += "</ol></dd></dl>";
            //    html += "</div></div>";
            //}
            //else
            //{
            //Bản desktop
            html = "<dl class=\"narrow-by\" id=\"_Hang\"><dt class=\"odd\">Hãng</dt> <dt class=\"toggle-tab mobile last even\" style=\"display: block\"></dt><dd class=\"Size toggle-content odd\"><ol class=\"\">";

            foreach (ManuItem re in result)
            {
                if (checkbool)
                {
                    if (id.Contains(re.ManufacturerId.ToString()))
                    {
                        html += "<li><a " + check + " href=\"" + Common.util.Function.InitUrlFilter(catUrl.Replace("-" + Common.util.Function.ConvertFileName(re.ManufacturerName), ""), n, -1, "mid", re.ManufacturerId.ToString()) + "\"><i class=\"iconcate-checkbox\"></i> <img width=\"70px\" height=\"30px\" src=\"" + re.Icon.Split('?')[0] + "medium/" + re.Icon.Split('?')[1] + "\"/></a></li>";
                        ViewData["choosedfilter"] += "<a  href=\"" + Common.util.Function.InitUrlFilter(catUrl.Replace("-" + Common.util.Function.ConvertFileName(re.ManufacturerName), ""), n, -1, "mid", re.ManufacturerId.ToString()) + "\" ><span>" + re.ManufacturerName + "</span><i class=\"iconcate-clearfil\"></i></a>";
                    }
                    else
                    {
                        html += "<li><a href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, 1, "mid", re.ManufacturerId.ToString()) + "\"><i class=\"iconcate-checkbox\"></i>  <img width=\"70px\" height=\"30px\" src=\"" + re.Icon.Split('?')[0] + "medium/" + re.Icon.Split('?')[1] + "\"/></a></li>";
                    }
                }
                else
                {
                    html += "<li><a href=\"" + Common.util.Function.InitUrlFilter(catUrl + "-" + Common.util.Function.ConvertFileName(re.ManufacturerName), n, 1, "mid", re.ManufacturerId.ToString()) + "\"><i class=\"iconcate-checkbox\"></i>  <img width=\"70px\" height=\"30px\" src=\"" + re.Icon.Split('?')[0] + "medium/" + re.Icon.Split('?')[1] + "\"/></a></li>";
                }
            }

            html += "</ol></dd></dl>";
            // }
            return html;
        }

        [PartialCacheAttribute("cache1hour")]
        protected string InitMenuCatalog(long ParentId, int depth)
        {
            List<Catalog> listCate = catalogBusiness.SearchCatalogByParentId(ParentId);
            string html = string.Empty;

            if (listCate.Count() > 0)
            {
                if (depth > 1) html = "<ul style=\"display: block;\">";
                foreach (Catalog re in listCate)
                {
                    if (re.Id == CatalogId1 || re.Id == CatalogId2 || re.Id == CatalogId3 || re.Id == CatalogId4)
                    {
                        html += "<li class=\"level" + depth + " active  parent\"><a href=\"" + Models.Common.Util.ReturnLinkFull(re.FriendlyUrl) + "-dm" + re.Id + "\">" + re.CatalogName + "</a><span class=\"expand\">expand</span>";

                        html += InitMenuCatalog(re.Id, depth + 1);
                    }
                    else
                    {
                        html += "<li class=\"level" + depth + "  parent\"><a  href=\"" + Models.Common.Util.ReturnLinkFull(re.FriendlyUrl) + "-dm" + re.Id + "\">" + re.CatalogName + "</a><span class=\"collapse\">collapse</span>";
                    }

                    html += "</li>";
                }
                if (depth > 1) html += "</ul>";
            }
            return html;
        }

        protected string InitFilterPrice(string catUrl, long id, string manuId, double priceFrom, double priceTo, string param)
        {
            //string[] listPriceValue = { "0-500000", "5000000-1000000", "1000000-2000000", "2000000-5000000", "5000000-1000000", "10000000-15000000", "15000000-0" };
            PriceAverage priceAverage1 = new PriceAverage("p1", "Dưới 5 triệu", 0, 5000000);
            PriceAverage priceAverage2 = new PriceAverage("p2", "Từ 5 - 7 triệu", 5000000, 7000000);
            PriceAverage priceAverage3 = new PriceAverage("p3", "Từ 7 - 10 triệu", 7000000, 10000000);
            PriceAverage priceAverage4 = new PriceAverage("p4", "Từ 10 - 12 triệu", 10000000, 12000000);
            PriceAverage priceAverage5 = new PriceAverage("p5", "Từ 12 - 15 triệu", 12000000, 15000000);
            PriceAverage priceAverage6 = new PriceAverage("p6", "Từ 15 - 20 triệu", 15000000, 20000000);
            PriceAverage priceAverage7 = new PriceAverage("p7", "Từ 20 - 50 triệu", 20000000, 50000000);
            PriceAverage priceAverage8 = new PriceAverage("p8", "Trên 50 triệu", 50000000, 1000000000);
            List<PriceAverage> ListAverage = new List<PriceAverage>();
            ListAverage.Add(priceAverage1);
            ListAverage.Add(priceAverage2);
            ListAverage.Add(priceAverage3);
            ListAverage.Add(priceAverage4);
            ListAverage.Add(priceAverage5);
            ListAverage.Add(priceAverage6);
            ListAverage.Add(priceAverage7);
            ListAverage.Add(priceAverage8);
            //string[] listPriceTile = { "≤ 500.000đ", "500.000 - 1.000.000đ", "1.000.000 - 2.000.000đ", "2.000.000 - 5.000.000đ", "5.000.000 - 10.000.000đ", "10.000.000 - 15.000.000đ", "≥ 15.000.000đ" };
            //double[] listPriceFrom = { 0, 500000, 1000000, 2000000, 5000000, 10000000, 15000000 };
            //double[] listPriceTo = { 500000, 1000000, 2000000, 5000000, 10000000, 15000000, 0 };

            string html = "";
            string[] arrayParam = null;
            NameValueCollection n = Request.QueryString;
            for (int j = 0; j < n.Count; j++)
            {
                string k1 = n.GetKey(j);
                string v = n.Get(j);
                if (k1 == "price")
                    arrayParam = v.Split(',');
            }

            int i = 0;

            //if (Request.Browser.IsMobileDevice)
            //{
            //    html += "<div class=\"btn-group dropup\"><button type=\"button\" class=\"btn btn-secondary dropdown-toggle\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">  Khoảng giá       </button> <div class=\"dropdown-menu\">";
            //    html += "<dl class=\"narrow-by\" _id=\"Price\"><dt class=\"odd\">Giá bán</dt> <dt class=\"toggle-tab mobile last even\" style=\"display: block\"></dt><dd class=\"Size toggle-content odd\"><ol class=\"\">";

            //    if (priceFrom == 0 && priceTo == 0)
            //    {
            //        foreach (PriceAverage s1 in ListAverage)
            //        {
            //            if (arrayParam != null)
            //            {
            //                bool boolprice = false;
            //                foreach (string re in arrayParam)
            //                {
            //                    if (re.Split(':')[0] == s1.PriceFrom.ToString() && re.Split(':')[1] == s1.PriceTo.ToString())
            //                    {
            //                        boolprice = true;
            //                    }
            //                }
            //                if (boolprice)
            //                {
            //                    html += "<li><a class=\"check\" href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, -1, "price", s1.PriceFrom + ":" + s1.PriceTo) + "\"><i class=\"iconcate-checkbox\"></i>" + s1.PriceTile + "</a></li>";
            //                    ViewData["choosedfilter"] += "<a href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, -1, "price", s1.PriceFrom + ":" + s1.PriceTo) + "\" ><span>" + s1.PriceTile + "</span><i class=\"iconcate-clearfil\"></i></a>";
            //                }
            //                else
            //                    html += "<li><a href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, 1, "price", s1.PriceFrom + ":" + s1.PriceTo) + "\"><i class=\"iconcate-checkbox\"></i>" + s1.PriceTile + "</a></li>";
            //            }
            //            else
            //                html += "<li> <a href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, 1, "price", s1.PriceFrom + ":" + s1.PriceTo) + "\"><i class=\"iconcate-checkbox\"></i>" + s1.PriceTile + "</a></li>";
            //            i++;
            //        }
            //    }

            //    html += "</ol></dd></dl>";
            //    html += "</div></div>";
            //}
            //else
            //{
            if (priceFrom == 0 && priceTo == 0)
            {
                html += "<dl class=\"narrow-by\" _id=\"Price\"><dt class=\"odd\">Giá bán</dt> <dt class=\"toggle-tab mobile last even\" style=\"display: block\"></dt><dd class=\"Size toggle-content odd\"><ol class=\"\">";

                foreach (PriceAverage s1 in ListAverage)
                {
                    if (arrayParam != null)
                    {
                        bool boolprice = false;
                        foreach (string re in arrayParam)
                        {
                            if (re.Split(':')[0] == s1.PriceFrom.ToString() && re.Split(':')[1] == s1.PriceTo.ToString())
                            {
                                boolprice = true;
                            }
                        }
                        if (boolprice)
                        {
                            html += "<li><a class=\"check\" href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, -1, "price", s1.PriceFrom + ":" + s1.PriceTo) + "\"><i class=\"iconcate-checkbox\"></i>" + s1.PriceTile + "</a></li>";
                            ViewData["choosedfilter"] += "<a href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, -1, "price", s1.PriceFrom + ":" + s1.PriceTo) + "\" ><span>" + s1.PriceTile + "</span><i class=\"iconcate-clearfil\"></i></a>";
                        }
                        else
                            html += "<li><a href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, 1, "price", s1.PriceFrom + ":" + s1.PriceTo) + "\"><i class=\"iconcate-checkbox\"></i>" + s1.PriceTile + "</a></li>";
                    }
                    else
                        html += "<li><a href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, 1, "price", s1.PriceFrom + ":" + s1.PriceTo) + "\"><i class=\"iconcate-checkbox\"></i>" + s1.PriceTile + "</a></li>";
                    i++;
                }
            }

            html += "</ol></dd></dl>";
            //   }
            return html;
        }

        protected string InitFilterAttribute(string catUrl, long id, string manuId, double priceFrom, double priceTo, string paramSQL, string param, string k)
        {
            string Dsthuoctinh = "";
            NameValueCollection n = Request.QueryString;
            for (int j = 0; j < n.Count; j++)
            {
                string k1 = n.GetKey(j);
                string v = n.Get(j);
                if (k1 != "mid" && k1 != "price" && k1 != "page" && k1 != "order" && k1 != "k")
                    Dsthuoctinh += v + ",";
            }

            CatalogPropertiesBusiness catalogPropertiesBusiness = new CatalogPropertiesBusiness();
            CatalogPropertiesValueBusiness catalogPropertiesValueBusiness = new CatalogPropertiesValueBusiness();

            List<CatalogProperty> listProperty = catalogPropertiesBusiness.SearchPropertiesByCatalogId(id);

            BuyGroup365Entities entitis = new BuyGroup365Entities();

            string html = "";
            string listName = "mid,price,";

            string ValuePropeties = "";
            if (Request.Browser.IsMobileDevice)
            {
                //Bản Mobile

                html += "<div class=\"btn-group dropup\"><button type=\"button\" class=\"btn btn-secondary dropdown-toggle\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">  Thuộc tính       </button> <div class=\"dropdown-menu\">";
                if (listProperty.Count() > 0)
                {
                    foreach (CatalogProperty re in listProperty)
                    {
                        string strQuery = BuildQueryFilterByPropery(id, manuId, priceFrom, priceTo, "", re.Id, k);

                        //var result = entitis.Database.SqlQuery<PropertyItem>(strQuery);

                        var result = catalogPropertiesValueBusiness.SearchPropertiesByPropertieId(re.Id);

                        if (result != null)// && result.Count() > 0)
                        {
                            html += "<dl class=\"narrow-by\" _id=\"" + re.Key.Replace("-", "_") + "\"><dt class=\"odd\">" + re.Name + "</dt> <dt class=\"toggle-tab mobile last even\" style=\"display: block\"></dt><dd class=\"Size toggle-content odd\"><ol class=\"\">";

                            listName += re.Key.Replace("-", "_") + ",";

                            foreach (CatalogPropertiesValue re1 in result)
                            {
                                string url = Common.util.Function.InitUrl(catUrl, id, manuId, priceFrom, priceTo, param);
                                string ValueUrl = Common.util.Function.ConvertFileName(re1.Value);
                                string itemUrl = ValueUrl;

                                if (Dsthuoctinh != "")
                                {
                                    if (Dsthuoctinh.Contains(ValueUrl) == true)
                                    {
                                        html += "<li><a class=\"check\" href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, -1, re.Key, ValueUrl) + "\"><i class=\"iconcate-checkbox\"></i>" + re1.Value + "</a></li>";
                                        ViewData["choosedfilter"] += "<a href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, -1, re.Key, ValueUrl) + "\" ><span>" + re1.Value + "</span><i class=\"iconcate-clearfil\"></i></a>";
                                    }
                                    else
                                        html += "<li><a href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, 1, re.Key, ValueUrl) + "\"><i class=\"iconcate-checkbox\"></i>" + re1.Value + "</a></li>";
                                }
                                else
                                    html += "<li><a href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, 1, re.Key, ValueUrl) + "\"><i class=\"iconcate-checkbox\"></i>" + re1.Value + "</a></li>";
                            }
                            html += "</ol></dd></dl>";
                        }
                    }
                }
                html += "<input type=\"hidden\" id=\"list_properties\" value=\"" + listName + "\"/><input type=\"hidden\" id=\"Url_Cata\" value=\"" + param + "\"/><input type=\"hidden\" id=\"listvalue_properties\" value=\"" + ValuePropeties + "\"/> ";
                html += "</div></div>";
            }
            else
            {
                //Bản desktop
                if (listProperty.Count() > 0)
                {
                    foreach (CatalogProperty re in listProperty)
                    {
                        string strQuery = BuildQueryFilterByPropery(id, manuId, priceFrom, priceTo, "", re.Id, k);

                        //var result = entitis.Database.SqlQuery<PropertyItem>(strQuery);

                        var result = catalogPropertiesValueBusiness.SearchPropertiesByPropertieId(re.Id);

                        if (result != null)// && result.Count() > 0)
                        {
                            html += "<dl class=\"narrow-by\" _id=\"" + re.Key.Replace("-", "_") + "\"><dt class=\"odd\">" + re.Name + "</dt> <ul class=\"filter-ul\">";
                            listName += re.Key.Replace("-", "_") + ",";

                            foreach (CatalogPropertiesValue re1 in result)
                            {
                                string url = Common.util.Function.InitUrl(catUrl, id, manuId, priceFrom, priceTo, param);
                                string ValueUrl = Common.util.Function.ConvertFileName(re1.Value);
                                string itemUrl = ValueUrl;

                                if (Dsthuoctinh != "")
                                {
                                    if (Dsthuoctinh.Contains(ValueUrl) == true)
                                    {
                                        html += "<li><a class=\"check\" href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, -1, re.Key, ValueUrl) + "\"><i class=\"iconcate-checkbox\"></i>" + re1.Value + "</a></li>";
                                        ViewData["choosedfilter"] += "<a href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, -1, re.Key, ValueUrl) + "\" ><span>" + re1.Value + "</span><i class=\"iconcate-clearfil\"></i></a>";
                                    }
                                    else
                                        html += "<li><a href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, 1, re.Key, ValueUrl) + "\"><i class=\"iconcate-checkbox\"></i>" + re1.Value + "</a></li>";
                                }
                                else
                                    html += "<li><a href=\"" + Common.util.Function.InitUrlFilter(catUrl, n, 1, re.Key, ValueUrl) + "\"><i class=\"iconcate-checkbox\"></i>" + re1.Value + "</a></li>";
                            }
                            html += "</ul></dl>";
                        }
                    }
                }

                html += "<input type=\"hidden\" id=\"list_properties\" value=\"" + listName + "\"/><input type=\"hidden\" id=\"Url_Cata\" value=\"" + param + "\"/><input type=\"hidden\" id=\"listvalue_properties\" value=\"" + ValuePropeties + "\"/> ";
            }
            return html;
        }

        protected string BuildQueryFilterByPropery(long? id, string mid, double? priceFrom, double? priceTo, string param, long CatalogPropertyId, string k)
        {
            string query = "", buildQuery = "";

            if (id != 0)
            {
                query = @"BEGIN
				             SELECT Value, ValueUrl, COUNT(Value) AS Total FROM
	                         Products JOIN ProductProperties ON Products.Id=ProductProperties.ProductId
		                     JOIN CatalogProperties ON CatalogProperties.Id=ProductProperties.CatalogPropertyId
		                        WHERE Products.CatalogId={0}
                                AND (CatalogPropertyId={5})
                                --AND (Status = 0 OR Status=1)
				                AND ManufacturerId in ({1}))
				                --AND ({2}=0 OR {2} <=Price)
				                --AND ({3}=0 OR {3} >=Price)
                                AND (ProductName Like '%{6}%' OR Tags Like '%{6}%')
				                {4}
                              GROUP BY  Value, ValueUrl

		                END";
                //if (order == 1)//Mới nhất

                buildQuery = string.Format(query, id, mid, priceFrom, priceTo, param, CatalogPropertyId, k);
            }

            return buildQuery;
        }

        [PartialCacheAttribute("cache1hour")]
        public ActionResult MenuCategory()
        {
            List<Catalog> listLevel1 = catalogBusiness.SearchCatalogByParentId(-1);
            string html = "";
            foreach (Catalog item in listLevel1)
            {
                List<Catalog> listcatasSub = catalogBusiness.SearchCatalogByParentId(item.Id);
                if (listcatasSub.Count > 0)
                {
                    string checktrung = "";
                    checktrung = Kiemtratrunglink(Request.RawUrl, item.Id);

                    html += "<li class=\"nav-item \"> <a class=\"nav-link\" href=\"" + Models.Common.Util.ReturnLinkFull(item.FriendlyUrl) + "\" >" + item.CatalogName + "</a> ";
                    //html += "<button class=\"toggle\"><i class=\"icon-angle-down\"></i></button>";
                    //html += "<ul private class=\"children\">";
                    //foreach (Catalog item1 in listcatasSub)
                    //{
                    //    List<Catalog> listcatasSub1 = catalogBusiness.SearchCatalogByParentId(item1.Id);
                    //    if (listcatasSub1.Count > 0)
                    //    {
                    //        string checktrung1 = "";
                    //        checktrung1 = Kiemtratrunglink(Request.RawUrl, item1.Id);

                    //        html += " <li class=\"cat-private item cat-item-" + item1.Id + " current-private cat cat-private parent has-child " + checktrung1 + "\" aria-expanded=\"false\"><a href=\"" + item1.FriendlyUrl + "\" >" + item1.CatalogName + "</a>";
                    //        //html += "<button class=\"toggle\"><i class=\"icon-angle-down\"></i></button>";
                    //        html += "<ul class=\"children\">";
                    //        foreach (Catalog item2 in listcatasSub1)
                    //        {
                    //            string checktrung2 = "";
                    //            checktrung2 = Kiemtratrunglink(Request.RawUrl, item2.Id);

                    //            html += "<li class=\"cat-item cat-item-" + item2.Id + " " + checktrung2 + "\"><a href=\"" + Models.Common.Util.ReturnLinkFull(item2.FriendlyUrl) + " \">" + item2.CatalogName + "</a></li>";
                    //        }
                    //        html += "   </ul>" +
                    //            "</li>";
                    //    }
                    //    else
                    //    {
                    //        html += "<li class=\"cat-item cat-item-" + item1.Id + "\"><a href=\"" + Models.Common.Util.ReturnLinkFull(item1.FriendlyUrl) + "\">" + item1.CatalogName + "</a></li>";
                    //    }
                    //}
                    //html += "</ul>" +
                    html += "</li>";
                }
                else
                {
                    html += "<li class=\"nav-item \"><a class=\"nav-link\" href=\"" + Models.Common.Util.ReturnLinkFull(item.FriendlyUrl) + "\">" + item.CatalogName + "</a></li>";
                }
            }
            ViewBag.MenuCatalog = html;
            return PartialView();
        }

        private string Kiemtratrunglink(string rawUrl, long cataid)
        {
            Catalog cat = new Catalog();
            cat = catalogBusiness.GetById(cataid);
            if (cat.FriendlyUrl == rawUrl.Replace("/", ""))
                return "current-cat active";
            else
            {
                List<Catalog> Listcata = new List<Catalog>();
                Listcata = catalogBusiness.SearchCatalogByParentId(cataid);
                foreach (Catalog re in Listcata)
                {
                    if (KiemtratrunglinkActive(rawUrl, re.Id) == "active")
                    {
                        return "active";
                    }
                    else
                    {
                        List<Catalog> Listcata1 = new List<Catalog>();
                        Listcata1 = catalogBusiness.SearchCatalogByParentId(re.Id);
                        foreach (Catalog re1 in Listcata1)
                        {
                            if (KiemtratrunglinkActive(rawUrl, re1.Id) == "active")
                            {
                                return "active";
                            }
                        }
                    }
                }
                return "";
            }
        }

        private string KiemtratrunglinkActive(string rawUrl, long cataid)
        {
            Catalog cat = new Catalog();
            cat = catalogBusiness.GetById(cataid);
            if (cat.FriendlyUrl == rawUrl.Replace("/", ""))
                return "active";
            else
                return "";
        }

        [PartialCacheAttribute("cache1hour")]
        public ActionResult Menudoc()
        {
            MenusBusiness menuBussiness = new MenusBusiness();

            string table1 = "Menus";
            string key = "MenuLevel1";
            List<Menus> listLevel1;
            if (HttpContext.Cache[key] == null)
            {
                SqlCacheDependency dep1 = new SqlCacheDependency(Database_Name, table1);
                AggregateCacheDependency aggDep = new AggregateCacheDependency();
                aggDep.Add(dep1);
                listLevel1 = menuBussiness.SearchMenusByParentId(-1, 1);
                HttpContext.Cache.Insert(key, listLevel1, aggDep);
            }
            else
            {
                listLevel1 = (List<Menus>)HttpContext.Cache[key];
            }

            string html = "";
            foreach (Menus item in listLevel1)
            {
                string UrlIcon = "";

                if (item.BackGround != null && item.BackGround != "" && item.IsBackGround == true)
                    UrlIcon = "<img width =\"20px\" height=\"20px\" style=\"float:left;margin-right: 5px;\" src=\"" + item.BackGround + "\"/>";

                html += "<li class=\"dropdown\" data-submenu-id=\"submenu-" + item.Id + "\"><span>" + UrlIcon + "<a href=\"" + Models.Common.Util.ReturnLinkFull(item.Link) + "\" class=\"menucap1\">" + item.MenuName + "</a> </span>";

                string keylistmenuSub = "listmenuSub" + item.Id.ToString();
                List<Menus> listmenuSub;
                if (HttpContext.Cache[keylistmenuSub] == null)
                {
                    SqlCacheDependency dep1 = new SqlCacheDependency(Database_Name, table1);
                    AggregateCacheDependency aggDep = new AggregateCacheDependency();
                    aggDep.Add(dep1);
                    listmenuSub = menuBussiness.SearchMenusByParentId(item.Id);
                    HttpContext.Cache.Insert(keylistmenuSub, listmenuSub, aggDep);
                }
                else
                {
                    listmenuSub = (List<Menus>)HttpContext.Cache[keylistmenuSub];
                }

                if (listmenuSub.Count > 0)
                {
                    html += "<div id=\"submenu-" + item.Id + "\" class=\"subcate gd-menu\" style=\"display: none;\">";
                    foreach (Menus item1 in listmenuSub)
                    {
                        html += "<aside class=\"autow\"><strong> <a href =\"" + Models.Common.Util.ReturnLinkFull(item1.Link) + "\" class=\"menucap1\">" + item1.MenuName + "</a></strong>";

                        string keylistmenuSub1 = "listmenuSub1" + item1.Id;
                        List<Menus> listmenuSub1;
                        if (HttpContext.Cache[keylistmenuSub1] == null)
                        {
                            SqlCacheDependency dep1 = new SqlCacheDependency(Database_Name, table1);
                            AggregateCacheDependency aggDep = new AggregateCacheDependency();
                            aggDep.Add(dep1);
                            listmenuSub1 = menuBussiness.SearchMenusByParentId(item1.Id);
                            HttpContext.Cache.Insert(keylistmenuSub1, listmenuSub1, aggDep);
                        }
                        else
                        {
                            listmenuSub1 = (List<Menus>)HttpContext.Cache[keylistmenuSub1];
                        }
                        if (listmenuSub1.Count > 0)
                        {
                            html += "<div>";
                            foreach (Menus item2 in listmenuSub1)
                            {
                                string UrlIcon1 = "";
                                if (item2.BackGround != null && item2.BackGround != "" && item2.IsBackGround == true)
                                    UrlIcon1 = "<img width =\"20px\" height=\"20px\" style=\"float:left;margin-right: 5px;\" src=\"" + item2.BackGround + "\"/>";

                                html += UrlIcon1 + " <a href=\"" + Models.Common.Util.ReturnLinkFull(item2.Link) + " \">" + item2.MenuName + "</a>";
                            }
                            html += "   </div>";
                        }
                        html += " </aside>";
                    }
                    html += "</div></li>";
                }
                else
                {
                    html += "</li>";
                }
            }
            ViewBag.MenuNgang = html;
            return PartialView();
        }

        [PartialCacheAttribute("cache1hour")]
        public void MenuTuVan()
        {
            string html = "";
            NewsGroupBusiness newsGroupBusiness = new NewsGroupBusiness();

            List<NewsGroups> listNewsGroupLevel1 = newsGroupBusiness.GetByParentId(10);

            foreach (NewsGroups item in listNewsGroupLevel1)
            {
                html += "<li>" +
                        "<a class=\"\" href=\"/tin-tuc/" + Common.util.Function.ConvertUrlString(item.NewsGroupName) + "-nt" + item.Id + ".html\">" + item.NewsGroupName + "</a>";
                html += "</li>";
            }
            ViewData["StrMenuNewsTuvan"] = html;
        }


        [PartialCacheAttribute("cache1hour")]
        public ActionResult IsNew()
        {

            return PartialView();
        }
        [PartialCacheAttribute("cache1hour")]
        public ActionResult IsHot()
        {

            return PartialView();
        }
        [PartialCacheAttribute("cache1hour")]
        public ActionResult Issale()
        {

            return PartialView();
        }
        [PartialCacheAttribute("cache1hour")]
        public ActionResult IsBanchay()
        {

            return PartialView();
        }
        [PartialCacheAttribute("cache1hour")]
        public ActionResult SliderHome()
        {

            return PartialView();
        }
        [PartialCacheAttribute("cache1hour")]
        public ActionResult NewsHome()
        {

            return PartialView();
        }
        [PartialCacheAttribute("cache1hour")]
        public ActionResult HomeSelectedCatalog()
        {
            string table1 = "Catalogs";
            string table2 = "Products";
            List<Catalog> listLevel1;
            if (HttpContext.Cache["listLevel1"] == null)
            {
                SqlCacheDependency dep1 = new SqlCacheDependency(Database_Name, table1);
                AggregateCacheDependency aggDep = new AggregateCacheDependency();
                aggDep.Add(dep1);
                listLevel1 = catalogBusiness.SearchCatalogByParentIdActive(-1);
                HttpContext.Cache.Insert("listLevel1", listLevel1, aggDep);
            }
            else
            {
                listLevel1 = (List<Catalog>)HttpContext.Cache["listLevel1"];
            }
            string html = "";
            foreach (Catalog item in listLevel1)
            {
                string bannerUrl = "";
                if (item.Banner != null && item.Banner != "")
                {
                    bannerUrl = item.Banner.Split('?')[0] + "Medium/" + item.Banner.Split('?')[1];
                }
                html += "<div class=\"row row-small catelogy-section\" style=\"margin-top: 10px;\"> <div class=\"col small-12 large-12\"><div class=\"col-inner\"><div class=\"catelogy-title\"><div class=\"catelogy-title-left\"> <h3><a href=\"" + Models.Common.Util.ReturnLinkFull(item.FriendlyUrl) + "\">" + item.CatalogName + "</a></h3></div>";
                //hiển thị danh mục con
                html += "<div class=\"catelogy-title-right hiden-mobile\"><ul>";
                List<Catalog> listcatasSub;
                string keylistcatasSub = "listcatasSub" + item.Id.ToString();
                if (HttpContext.Cache[keylistcatasSub] == null)
                {
                    SqlCacheDependency dep1 = new SqlCacheDependency(Database_Name, table1);
                    AggregateCacheDependency aggDep = new AggregateCacheDependency();
                    aggDep.Add(dep1);
                    listcatasSub = catalogBusiness.SearchCatalogByParentId(item.Id);

                    HttpContext.Cache.Insert(keylistcatasSub, listcatasSub, aggDep);
                }
                else
                {
                    listcatasSub = (List<Catalog>)HttpContext.Cache[keylistcatasSub];
                }

                foreach (Catalog item1 in listcatasSub)
                {
                    html += " <li> <a href=\"" + Models.Common.Util.ReturnLinkFull(item1.FriendlyUrl) + "\" title=\"" + item1.CatalogName + "\">" + item1.CatalogName + " </a></li>";
                }
                html += "</ul> </div>  <div class=\"clearboth\"> </div></div>";

                html += " <div class=\"row large-columns-5 medium-columns-3 small-columns-2 row-collapse has-shadow row-box-shadow-1 row-box-shadow-2-hover\" itemprop=\"hasOfferCatalog\" itemscope=\"\" itemtype=\"http://schema.org/OfferCatalog\">";
                //if (item.Banner != null && item.Banner != "")
                //{
                //    html += "<a  href=\"" + item.FriendlyUrl + "\"><img width=\"100%\" src=\"" + bannerUrl + "\"></a> <div class=\"clear\"></div>";
                //}
                List<SearchProductByType_Result> listproduct;
                string keylistproduct = "listproduct" + item.Id.ToString();
                if (HttpContext.Cache[keylistproduct] == null)
                {
                    SqlCacheDependency dep1 = new SqlCacheDependency(Database_Name, table2);
                    AggregateCacheDependency aggDep = new AggregateCacheDependency();
                    aggDep.Add(dep1);
                    listproduct = productBussiness.ListByProductsIdCatalogID((int)item.Id).Where(x => x.IsHome = true).Take(4).ToList();

                    HttpContext.Cache.Insert(keylistproduct, listproduct, aggDep);
                }
                else
                {
                    listproduct = (List<SearchProductByType_Result>)HttpContext.Cache[keylistproduct];
                }

                foreach (SearchProductByType_Result re in listproduct)
                {
                    string probig = "";

                    string imgPro = "";
                    try
                    {
                        //  imgPro = re.ImgSource.Replace("Uploadimages", "Upload_thumbs/Images");
                        imgPro = re.ImgSource;
                    }
                    catch { }
                    if (re.IsProductBig)
                    {
                        if (!Request.Browser.IsMobileDevice)
                        {
                            imgPro = re.LongImages;
                            probig = "product-big";
                        }
                        else
                        {
                            imgPro = re.ImgSource;
                            probig = "";
                        }
                    }
                    int phantramKM = 0;
                    try
                    {
                        phantramKM = Convert.ToInt32((re.Cost - re.Price) / re.Cost * 100);
                    }
                    catch { }
                    html += "<div class=\"product-small " + probig + " col has-hover post-4494 product type-product status-publish has-post-thumbnail product_cat-may-chieu-optoma first instock sale shipping-taxable purchasable product-type-simple\"  itemprop=\"itemListElement\" itemscope=\"\" itemtype=\"http://schema.org/OfferCatalog\">";
                    html += "<div class=\"col-inner\"><div class=\"badge-container absolute left top z-1\"><div class=\"callout badge badge-square\">";
                    if (phantramKM > 0)
                        html += "<div class=\"badge-inner secondary on-sale\"><span class=\"onsale\">-" + phantramKM + "%</span></div>";
                    html += "</div></div><div class=\"product-small box \">";
                    //if (re.IsVip)
                    //    html += "<span class=\"product-gift\"><i class=\"fa fa-gift\"></i> Quà tặng </span>";
                    html += "<div class=\"box-image\"><div class=\"image-zoom\"><a title=\"" + re.ProductName + "\" href=\"" + Models.Common.Util.ReturnLinkFull(re.FriendlyUrl) + "\">" +             // hien thi ten san pham ngan gon
                    "<img width=\"300\" itemprop=\"image\" height=\"300\" src=\"" + imgPro + "\" class=\"attachment-woocommerce_thumbnail size-woocommerce_thumbnail\" alt=\"\" ></a></div><div class=\"image-tools is-small top right show-on-hover\"></div><div class=\"image-tools grid-tools text-center hide-for-small bottom hover-slide-in show-on-hover\"></div></div> ";
                    html += "<div class=\"box-text box-text-products\"><div class=\"title-wrapper\"> <p class=\"name product-title\"  itemprop=\"name\"><a title=\"" + re.ProductName + "\" href=\"" + Models.Common.Util.ReturnLinkFull(re.FriendlyUrl) + "\">" + re.ProductName + "</a></p> </div>";
                    html += "<div class=\"price-wrapper\"><span class=\"price\">";
                    if (re.Price != 0)
                    {
                        html += "<del><span class=\"woocommerce-Price-amount amount\">" + String.Format("{0:#,###}", re.Cost) + "<span class=\"woocommerce-Price-currencySymbol\">₫</span></span></del>";
                        html += "<ins><span class=\"woocommerce-Price-amount amount\">" + String.Format("{0:#,###}", re.Price) + "<span class=\"woocommerce-Price-currencySymbol\">₫</span></span></ins>";
                    }
                    else
                    {
                        html += "<ins><span class=\"woocommerce-Price-amount amount\">Liên hệ</span></span></ins>";
                    }

                    html += "</span> </div> </div></div></div></div>";
                }

                html += "</div></div></div>  <style scope=\"scope\" ></style></div>";
            }
            ViewBag.HomeSelectedCatalog = html;
            return PartialView();
        }

        public ActionResult MenuNew()
        {
            List<NewsGroups> listLevel1 = newgroupBussiness.SearchNewGroupsByParentId(-1);
            string html = "";
            foreach (NewsGroups item in listLevel1)
            {
                List<NewsGroups> listcatasSub = newgroupBussiness.SearchNewGroupsByParentId(item.Id);
                if (listcatasSub.Count > 0)
                {
                    string checktrung = "";
                    //  checktrung = Kiemtratrunglink(Request.RawUrl, item.Id);

                    html += "<li class=\"cat-item cat-item-53 cat-parent has-child " + checktrung + "\" data-submenu-id=\"submenu-" + item.Id + "\"> <a href=\"" + Models.Common.Util.ReturnLinkFull(item.FriendlyUrl) + "\" >" + item.NewsGroupName + "</a> ";
                    //html += "<button class=\"toggle\"><i class=\"icon-angle-down\"></i></button>";
                    html += "<ul private class=\"children\">";
                    foreach (NewsGroups item1 in listcatasSub)
                    {
                        List<NewsGroups> listcatasSub1 = newgroupBussiness.SearchNewGroupsByParentId(item1.Id);
                        if (listcatasSub1.Count > 0)
                        {
                            string checktrung1 = "";
                            //   checktrung1 = Kiemtratrunglink(Request.RawUrl, item1.Id);

                            html += " <li class=\"cat-private item cat-item-" + item1.Id + " current-private cat cat-private parent has-child " + checktrung1 + "\" aria-expanded=\"false\"><a href=\"" + item1.FriendlyUrl + "\" >" + item1.NewsGroupName + "</a>";
                            //html += "<button class=\"toggle\"><i class=\"icon-angle-down\"></i></button>";
                            html += "<ul class=\"children\">";
                            foreach (NewsGroups item2 in listcatasSub1)
                            {
                                string checktrung2 = "";
                                //   checktrung2 = Kiemtratrunglink(Request.RawUrl, item2.Id);

                                html += "<li class=\"cat-item cat-item-" + item2.Id + " " + checktrung2 + "\"><a href=\"" + Models.Common.Util.ReturnLinkFull(item2.FriendlyUrl) + " \">" + item2.NewsGroupName + "</a></li>";
                            }
                            html += "   </ul>" +
                                "</li>";
                        }
                        else
                        {
                            html += "<li class=\"cat-item cat-item-" + item1.Id + "\"><a href=\"" + Models.Common.Util.ReturnLinkFull(item1.FriendlyUrl) + "\">" + item1.NewsGroupName + "</a></li>";
                        }
                    }
                    html += "</ul>" +
                        "</li>";
                }
                else
                {
                    html += "<li class=\"cat-item cat-item-" + item.Id + "\"><a href=\"" + Models.Common.Util.ReturnLinkFull(item.FriendlyUrl) + "\">" + item.NewsGroupName + "</a></li>";
                }
            }
            ViewBag.MenuNew = html;
            return PartialView();
        }

        public class ManuItem
        {
            public long ManufacturerId { get; set; }
            public string ManufacturerName { get; set; }
            public string Icon { get; set; }
            public int Total { get; set; }
        }

        public class PropertyItem
        {
            public string Value { get; set; }
            public string ValueUrl { get; set; }
            public int Total { get; set; }
        }

        public ActionResult ListProductAttractive()
        {
            ProductsBusiness productBusiness = new ProductsBusiness();
            var temp = productBusiness.GetByAttactive(12);
            //ViewData["ListProduct"] = temp;
            return PartialView(temp);
        }

        [PartialCacheAttribute("cache1Day")]
        public ActionResult MenuTop()
        {
            MenusBusiness menubussiness = new MenusBusiness();
            List<Common.Menus> ListMenu = new List<Common.Menus>();
            List<Common.Menus> ListMenu1 = new List<Common.Menus>();
            List<Common.Menus> ListMenu2 = new List<Common.Menus>();
            string html = "";

            ListMenu = menubussiness.SearchMenusByParentId(-1, 2);
            foreach (var re in ListMenu)
            {
                ListMenu1 = menubussiness.SearchMenusByParentId(re.Id);
                if (ListMenu1.Count > 0)
                {
                    html += " <li class=\"menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children has-dropdown\">";
                    html += "   <a class=\"nav-top-link\" href=\"" + Models.Common.Util.ReturnLinkFull(re.Link) + "\">" + re.MenuName + "<i class=\"icon-angle-down\"></i></a>";

                    html += "<ul class=\"nav-dropdown nav-dropdown-simple dark\">";
                    foreach (var re1 in ListMenu1)
                    {
                        html += " <li class=\"menu-item menu-item-type-taxonomy menu-item-object-product_cat\">";
                        html += "  <a href=\"" + Models.Common.Util.ReturnLinkFull(re1.Link) + "\">" + re1.MenuName + "</a>";
                        ListMenu2 = menubussiness.SearchMenusByParentId(re1.Id);
                        if (ListMenu2.Count > 0)
                        {
                            html += "<ul class=\"level2\">";
                            foreach (var re2 in ListMenu2)
                            {
                                html += "<li><a href=\"" + Models.Common.Util.ReturnLinkFull(re2.Link) + "\">" + re2.MenuName + "</a></li>";
                            }
                            html += " </ul>";
                        }
                        html += "  </li>";
                    }

                    html += "</ul>";
                    html += "</li>";
                }
                else
                {
                    if (re.MenuName != "Trang chủ")
                    {
                        html += " <li class=\"menu-item menu-item-type-taxonomy menu-item-object-category\">";
                        html += "   <a class=\"nav-top-link\" href=\"" + Models.Common.Util.ReturnLinkFull(re.Link) + "\">" + re.MenuName + "</a>";
                        html += "</li>";
                    }
                    else
                    {
                        html += " <li class=\"menu-item menu-item-type-taxonomy menu-item-object-category menu-item-home\">";
                        html += "   <a class=\"nav-top-link\" href=\"" + Models.Common.Util.ReturnLinkFull(re.Link) + "\">" + re.MenuName + "</a>";
                        html += "</li>";
                    }
                }
            }

            ViewBag.MenuTop = html;
            return PartialView();
        }

        [PartialCacheAttribute("cache1Day")]
        public ActionResult MenuTopMobile()
        {
            MenusBusiness menubussiness = new MenusBusiness();
            List<Common.Menus> ListMenu = new List<Common.Menus>();
            List<Common.Menus> ListMenu1 = new List<Common.Menus>();
            List<Common.Menus> ListMenu2 = new List<Common.Menus>();
            string html = "";

            ListMenu = menubussiness.SearchMenusByParentId(-1, 3);
            foreach (var re in ListMenu)
            {
                ListMenu1 = menubussiness.SearchMenusByParentId(re.Id);
                if (ListMenu1.Count > 0)
                {
                    html += " <li class=\"menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children menu-item-" + re.Id + " has-child\">";
                    html += "   <a class=\"nav-top-link\" href=\"" + Models.Common.Util.ReturnLinkFull(re.Link) + "\">" + re.MenuName + "</a>";

                    html += "<ul class=\"children\">";
                    foreach (var re1 in ListMenu1)
                    {
                        html += " <li class=\"menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-" + re1.Id + "\">";
                        html += "  <a href=\"" + Models.Common.Util.ReturnLinkFull(re1.Link) + "\">" + re1.MenuName + "</a>";
                        //ListMenu2 = menubussiness.SearchMenusByParentId(re1.Id);
                        //if (ListMenu2.Count > 0)
                        //{
                        //    html += "<ul class=\"level2\">";
                        //    foreach (var re2 in ListMenu2)
                        //    {
                        //        html += "<li><a href=\"/" + re2.Link + "\">" + re2.MenuName + "</a></li>";
                        //    }
                        //    html += " </ul>";
                        //}
                        html += "  </li>";
                    }

                    html += "</ul>";
                    html += "</li>";
                }
                else
                {
                    if (re.MenuName != "Trang chủ")
                    {
                        html += " <li class=\"menu-item menu-item-type-post_type menu-item-object-page menu-item-home current-menu-item page_item page-item-2 current_page_item menu-item-" + re.Id + "\">";
                        html += "   <a class=\"nav-top-link\" href=\"" + Models.Common.Util.ReturnLinkFull(re.Link) + "\">" + re.MenuName + "</a>";
                        html += "</li>";
                    }
                    else
                    {
                        html += " <li class=\"menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children menu-item-" + re.Id + "\">";
                        html += "   <a class=\"nav-top-link\" href=\"" + Models.Common.Util.ReturnLinkFull(re.Link) + "\">" + re.MenuName + "</a>";
                        html += "</li>";
                    }
                }
            }

            ViewBag.MenuTopMobile = html;
            return PartialView();
        }

        [PartialCacheAttribute("cache1Day")]
        public ActionResult NewsFooter()
        {
            return PartialView();
        }

        public string GetByCodeHtml(string Code)
        {
            try
            {
                string tableName = "TextHtml";
                if (HttpContext.Cache[Code] == null)
                {
                    TextHtmlBusiness textHtmlBussiness = new TextHtmlBusiness();
                    SqlCacheDependency dep1 = new SqlCacheDependency(Database_Name, tableName);
                    AggregateCacheDependency aggDep = new AggregateCacheDependency();
                    aggDep.Add(dep1);
                    string Content = textHtmlBussiness.GetByCode(Code).Content;
                    HttpContext.Cache.Insert(Code, Content, aggDep);
                }
                return (string)HttpContext.Cache[Code];
            }
            catch
            {
                return "";
            }
        }
    }
}