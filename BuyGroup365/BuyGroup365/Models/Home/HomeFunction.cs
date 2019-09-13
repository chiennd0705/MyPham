using Business.ClassBusiness;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuyGroup365.Models.Home
{
    public class HomeFunction
    {
        public static List<PropertyList> GetPropertyByProduct(List<ProductProperty> listobj)
        {
            List<PropertyList> listResults = new List<PropertyList>();
            var liststring = listobj.Select(x => x.Name).Distinct().ToList();
            foreach (var item in liststring)
            {
                PropertyList entity = new PropertyList();
                entity.Name = item;
                var value = "";
                foreach (var item2 in listobj)
                {
                    if (item2.Name == item)
                    {
                        value += item2.Value + ", ";
                    }
                }
                value = value.TrimEnd();
                value = value.TrimEnd();
                entity.Value = value;
                listResults.Add(entity);
            }
            return listResults;
        }

        public static string RandomString(int size)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static string GetLinkCdn()
        {
            Business.ClassBusiness.SystemSettingBusiness systemSetting = new Business.ClassBusiness.SystemSettingBusiness();
            string link = systemSetting.GetByKey("cdn").FirstOrDefault().Value + "/";
            return link;
        }

        public static String InitPromotion(Product item)
        {
            CatalogsBusiness cataBussiness = new CatalogsBusiness();
            Catalog cata = new Catalog();
            string html = "";
            int i = 1;
            long cataid;
            try
            {
                if (item.PromotionID != null)
                {
                    html = "<strong>" + item.PromotionList.Title + " </strong>";

                    foreach (PromotionItem pr in item.PromotionList.PromotionItem)
                    {
                        html += " <span class=\"promo\" data-id=\"0\">";
                        html += "<i class=\"numeric\">" + i + "</i>" + pr.Title;
                        i++;
                        html += "</span>";
                    }
                    html += "<div style=\"margin: 8px;    border-top: 1px dashed;    padding-top: 5px;\">" + item.PromotionList.Description + "</div>";
                }
                else
                {
                    cataid = item.CatalogProducts.First().CatalogId;
                    cata = cataBussiness.GetById(cataid);
                    html = "<strong>" + cata.PromotionList.Title + " </strong>";

                    foreach (PromotionItem pr in cata.PromotionList.PromotionItem)
                    {
                        html += " <span class=\"promo\" data-id=\"0\">";
                        html += "<i class=\"numeric\">" + i + "</i>" + pr.Title;
                        i++;
                        html += "</span>";
                    }
                    html += "<div style=\"margin: 8px;    border-top: 1px dashed;    padding-top: 5px;\">" + cata.PromotionList.Description + "</div>";
                }
            }
            catch (Exception ex)
            { }
            return html;
        }

        public static PromotionList InitPromotionList(Product item)
        {
            CatalogsBusiness cataBussiness = new CatalogsBusiness();
            Catalog cata = new Catalog();
            string html = "";
            int i = 1;
            long cataid;
            try
            {
                if (item.PromotionID != null)
                {
                    return item.PromotionList;
                }
                else
                {
                    cataid = item.CatalogProducts.First().CatalogId;
                    cata = cataBussiness.GetById(cataid);
                    return cata.PromotionList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

public class PropertyList
{
    public string Name { get; set; }
    public string Value { get; set; }
}