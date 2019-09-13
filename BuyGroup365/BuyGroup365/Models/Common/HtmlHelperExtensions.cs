using System;
using System.Web.Mvc;

namespace BuyGroup365.Models.Common
{
    public class CustomHelpersMvc
    {
        public static string TextBox(HtmlHelper htmlHelper, string name, string value, string classelement, string type)
        {
            return String.Format("<input id='{0}’ name='{0}’ value='{1}’ type='{3}' class='{2}' />", name, value, classelement, type);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="dayselected">Ngày dc chọn</param>
        /// <param name="mounthselected">Tháng được chọn</param>
        /// <param name="yearselected">Năm được chọn</param>
        /// <param name="classelement">Class cho dropdown</param>
        /// <param name="fromyear">Sô năm cach năm hiện tại</param>
        /// <param name="endyear"></param>
        /// <returns></returns>
        public static string DateDropdownlist(HtmlHelper htmlHelper, int? dayselected, int? mounthselected, int? yearselected, string classelement, int fromyear, int endyear)
        {
            var html = "";
            html += "<select id=\"day\" class=\"" + classelement + "\" style=\"width: 25%; margin-right: 15px;\" name=\"day\" >";
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
                if (dayselected != null && dayselected == d)
                {
                    html += "<option  selected=\"selected\"  value=\"" + str + "\">" + str + "</option>";
                }
                else
                {
                    html += "<option value=\"" + str + "\">" + str + "</option>";
                }
            }
            html += "</select>";

            html += "<select id=\"mouth\" class=\"" + classelement + "\" name=\"mouth\" style=\"width: 25%; margin-right: 15px;\">";
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

                if (mounthselected != null && mounthselected == d)
                {
                    html += "<option  selected=\"selected\" value=\"" + str + "\">" + str + "</option>";
                }
                else
                {
                    html += "<option value=\"" + str + "\">" + str + "</option>";
                }
            }
            html += "</select>";

            html += "<select id=\"year\" class=\"" + classelement + "\" name=\"year\" style=\"width: 25%;\">";
            html += "<option value=\"-1\">Năm</option>";
            var yearnow = DateTime.Now.Year;
            for (int d = yearnow - endyear; d >= yearnow - fromyear; d--)
            {
                if (yearselected != null && yearselected == d)
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
    }
}