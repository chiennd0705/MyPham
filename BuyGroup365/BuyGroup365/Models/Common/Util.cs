using Business.ClassBusiness;
using BuyGroup365.Areas.Manage.Controllers;
using BuyGroup365.Controllers;
using Common;
using Common.util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuyGroup365.Models.Common
{
    public class Util
    {
        private readonly List<Catalog> _listCate = null;
        private static String LinkCDN = ConfigurationManager.AppSettings["LinkCDN"];
        private static String HostUrl = ConfigurationManager.AppSettings["HostUrl"];

        public Util()
        {
            if (MemberController.List != null)
            {
                _listCate = MemberController.List;
            }
            else if (CategoryProductController.ListCate != null)
            {
                _listCate = CategoryProductController.ListCate;
            }
        }

        public string HtmlCate(long prentid, long? id)
        {
            int i = 0;
            string html = "<ul>";
            //var List = MemberController.List;
            List<Catalog> listobj = _listCate.Where(x => x.ParentId == prentid).ToList();//Test Cache dữ liêu.
            {
                foreach (Catalog item in listobj)
                {
                    if (id != null && item.Id == id)
                    {
                        html += "<li><input type=\"checkbox\" checked=\"checked\" id=\"item-" + item.Id +
                                "\"   onclick=\"GetValueManuface(" + item.Id +
                                ")\" /><label class=\"selectcate\" id=\"label-" + item.Id + "\"  for=\"item-" + item.Id +
                                "\"  onclick=\"GetValueManuface(" + item.Id + ")\" >" + item.CatalogName + "</label>";

                        i++;
                        string str = HtmlbyCate(item.Id, ref i, id);
                        html += str;

                        html += "</li>";
                    }
                    else
                    {
                        html += "<li><input type=\"checkbox\" id=\"item-" + item.Id + "\"   onclick=\"GetValueManuface(" +
                                item.Id + ")\" /><label id=\"label-" + item.Id + "\"  for=\"item-" + item.Id +
                                "\"  onclick=\"GetValueManuface(" + item.Id + ")\" >" + item.CatalogName + "</label>";

                        i++;
                        string str = HtmlbyCate(item.Id, ref i, id);
                        html += str;

                        html += "</li>";
                    }
                }
            }
            html += "</ul>";
            return html;
        }

        public string HtmlCateCheckBox(long prentid, long? id)
        {
            int i = 0;
            string html = "<ul>";
            //var List = MemberController.List;
            List<Catalog> listobj = _listCate.Where(x => x.ParentId == prentid).ToList();//Test Cache dữ liêu.
            {
                foreach (Catalog item in listobj)
                {
                    html += "   <div class=\"checkbox\">            <label>" +
                          " <input type=\"checkbox\"" +
                           "  name=\"" + item.CatalogName +
                         " value=\"" + item.Id + " /> ";
                }
            }
            html += "</ul>";
            return html;
        }

        public string HtmlbyCate(long cateId, ref int i, long? idmember)
        {
            string html = "<ul>";
            //   List<Catalog> listobj = _catalogsBusiness.GetDynamicQuery().Where(x => x.ParentId == cateId).ToList();
            List<Catalog> listobj = _listCate.Where(x => x.ParentId == cateId).ToList();//Test cache dữ liệu
            if (listobj.Any())
            {
                foreach (Catalog item in listobj)
                {
                    if (idmember != null && item.Id == idmember)
                    {
                        html += "<li><input type=\"checkbox\" checked=\"checked\" id=\"item-" + item.Id +
                                "\" onclick=\"GetValueManuface(" + item.Id +
                                ")\" /><label class=\"selectcate\"  id=\"label-" + item.Id + "\" for=\"item-" + item.Id +
                                "\"   onclick=\"GetValueManuface(" + item.Id + ")\" >" + item.CatalogName + "</label>";
                        i++;
                        string str = HtmlbyCate(item.Id, ref i, idmember);
                        html += str;
                        html += "</li>";
                    }
                    else
                    {
                        html += "<li><input type=\"checkbox\" id=\"item-" + item.Id + "\" onclick=\"GetValueManuface(" +
                                item.Id + ")\" /><label  id=\"label-" + item.Id + "\" for=\"item-" + item.Id +
                                "\"   onclick=\"GetValueManuface(" + item.Id + ")\" >" + item.CatalogName + "</label>";
                        i++;
                        string str = HtmlbyCate(item.Id, ref i, idmember);
                        html += str;
                        html += "</li>";
                    }
                }
            }

            html += "</ul>";
            return html;
        }

        public static string ReturnLink(string FriendlyUrl)
        {
            return "/" + FriendlyUrl;
            //  return HostUrl + FriendlyUrl;
        }

        public static string ReturnLinkFull(string FriendlyUrl)
        {
            return HostUrl + FriendlyUrl;
        }

        public static string ReturnFriendlyLink(string FriendlyUrl)
        {
            return "/" + Function.ConvertFileName(FriendlyUrl);
        }

        public static string ReturnLinkAuthor(string author)
        {
            try
            {
                return HostUrl + "/author/" + Function.ConvertFileName(author) + ".html";
            }
            catch
            {
                return "";
            }
        }

        public static string ReturnLinkCDN(string imageSource, bool isMobile)
        {
            //if (isMobile == true)
            //    return LinkCDN + imageSource.Replace("/Images", "/Images/_thumbs/Images");
            //else
            return LinkCDN + imageSource;
        }

        #region Shop

        public List<SelectListItem> LoadComBoShop()
        {
            var listItems = new List<SelectListItem>();
            var listShop = new ShopsBusiness().GetDynamicQuery();
            listItems.Add(new SelectListItem { Text = "--Chọn Shop--", Value = "-1" });
            foreach (var item in listShop)
            {
                listItems.Add(new SelectListItem { Text = item.ShopName, Value = item.Id.ToString() });
            }
            return listItems;
        }

        public static int CheckFileImage(HttpPostedFileBase fileBase)
        {
            if (fileBase != null && fileBase.ContentLength > 0)
            {
                string ext = System.IO.Path.GetExtension(fileBase.FileName);
                ext = ext.ToUpper();
                if (fileBase.ContentLength > 1000000)
                {
                    return 0;// File quá lớn
                }
                if (ext == ".JPG" || ext == ".PNG" || ext == ".GIF" || ext == ".JPEG")
                {
                    return 2;//  đúng định dạng
                }
                else
                {
                    return 1;// File không đúng định dạng
                }
            }
            else
            {
                return 2;
            }
            return 2;
        }

        #endregion Shop

        #region Dropdate birthday

        public static string LoadComBoDate(int? day, int? mounth, int? year)
        {
            var html = "";
            html += "<select id=\"day\" class=\"input datebirthday\" style=\"width: 25%; margin-right: 15px;\" name=\"day\" >";
            html += "<option value=\"-1\">Ngày </option>";
            for (int d = 1; d <= 31; d++)
            {
                var str = "";
                if (d < 10)
                {
                    str = "0" + d;
                }
                else
                {
                    str = d.ToString();
                }
                if (day != null && day == d)
                {
                    html += "<option  selected=\"selected\" value=\"" + str + "\">" + str + "</option>";
                }
                else
                {
                    html += "<option value=\"" + str + "\">" + str + "</option>";
                }
            }
            html += "</select>";

            html += "<select id=\"mouth\" class=\"input\" name=\"mouth\" style=\"width: 25%; margin-right: 15px;\">";
            html += "<option value=\"-1\">Tháng</option>";
            for (int d = 1; d <= 12; d++)
            {
                var str = "";
                if (d < 10)
                {
                    str = "0" + d;
                }
                else
                {
                    str = d.ToString();
                }

                if (mounth != null && mounth == d)
                {
                    html += "<option  selected=\"selected\" value=\"" + str + "\">" + str + "</option>";
                }
                else
                {
                    html += "<option value=\"" + str + "\">" + str + "</option>";
                }
            }
            html += "</select>";

            html += "<select id=\"year\" class=\"input\" name=\"year\" style=\"width: 25%;\">";
            html += "<option value=\"-1\">Năm</option>";
            var yearnow = DateTime.Now.Year;
            for (int d = yearnow - 10; d >= yearnow - 70; d--)
            {
                if (year != null && year == d)
                {
                    html += "<option selected=\"selected\" value=\"" + d + "\">" + d + "</option>";
                }
                else
                {
                    html += "<option value=\"" + d + "\">" + d + "</option>";
                }
            }
            html += "</select>";
            return html;
        }

        #endregion Dropdate birthday

        public static string JSSitting()
        {
            OrderSittingBusiness orderSittingBussiness = new OrderSittingBusiness();
            List<OrderSitting> ListorderSitting = new List<OrderSitting>();
            ListorderSitting = orderSittingBussiness.GetList("");
            string phone = "['";
            string mytimeAgo = "['";
            string people = "['";
            string country = "['";
            string Result = "";
            foreach (OrderSitting re in ListorderSitting)
            {
                phone += re.SDT + "','";
                mytimeAgo += re.TimeNote + "','";
                people += re.FullName + "','";
                country += re.City + "','";
            }
            try
            {
                phone = phone.Substring(0, phone.Length - 3);
                mytimeAgo = mytimeAgo.Substring(0, mytimeAgo.Length - 3);
                people = people.Substring(0, people.Length - 3);
                country = country.Substring(0, country.Length - 3);
            }
            catch { }
            phone += "']";
            mytimeAgo += "']";
            people += "']";
            country += "']";

            Result +=
                "$(function () {  " +
                    "setInterval(function () {  " +
                      "  $('#someone-purchased > div:first')  " +
                          "  .fadeOut(200)   " +
                          "  .next()  " +
                         "   .fadeIn(0)  " +
                         "   .end()  " +
                          "  .appendTo('#someone-purchased');" +
            "var phone =" + phone + ";" +
            "var mytimeAgo =" + mytimeAgo + " ;" +
           " var people =" + people + ";" +
           " var country =" + country + ";" +
           " var randomlytimeAgo = Math.floor(Math.random() * mytimeAgo.length);" +
           " var randomlytimeAgo1 = Math.floor(Math.random() * people.length);" +
           " var randomlytimeAgo2 = Math.floor(Math.random() * country.length);" +
           " var randomlytimeAgo3 = Math.floor(Math.random() * phone.length);" +
           " var currentmytimeAgo = mytimeAgo[randomlytimeAgo];" +
           " var currentpeople = people[randomlytimeAgo1];" +
           " var currentcountry = country[randomlytimeAgo2];" +
           " var currentphone = phone[randomlytimeAgo3];" +
           " $(\".timeAgo\").text(currentmytimeAgo + \" trước\");" +
          "  $(\".people\").text(currentpeople);" +
          "  $(\".country\").text(currentcountry);" +
           " $(\".phone-order\").text(currentphone);" +
         "  }, 20000);" +
             "   setInterval(function () {" +
        "    $('#someone-purchased').fadeIn(function () { $(this).removeClass(\"fade-out\"); }).delay(5000).fadeIn(function () { $(this).addClass(\"fade-out\"); }).delay(5000);" +
       " }, 20000);" +
        " });";

            return Result;
        }
    }
}