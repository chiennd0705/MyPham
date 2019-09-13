namespace BuyGroup365.Areas.Manage.Models
{
    public class AdminFunction
    {
        public static string GetUrlImage(string url, string folder)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var a = url.IndexOf('?');
                if (a != -1)
                {
                    var split = url.Split('?');
                    return split[0] + folder + "/" + split[1];
                }
                else
                {
                    return "/img/noimage.png";
                }
            }
            else
            {
                return "/img/noimage.png";
            }
        }

        public static string BindHeader(string levelone, string leveltwo, string urlone, string urltow, string levelthree)
        {
            var html = "";
            if (string.IsNullOrEmpty(leveltwo))
            {
                html = "  <div class=\"row\">" +
           "<div class=\"col-lg-12\">" +
               "<ol class=\"breadcrumb\">" +
                   "<li class=\"active\" style=\"font-size: 16px;\">" +
            "<a href=\"/Manage/Admin\"><i class=\"fa fa-home\"></i> Dashboard</a> <i class=\"fa fa-angle-right\"></i> " + levelone + "</li>" +
               "</ol> </div>   </div>";
            }
            else if (!string.IsNullOrEmpty(leveltwo))
            {
                html = "  <div class=\"row\">" +
           "<div class=\"col-lg-12\">" +
               "<ol class=\"breadcrumb\">" +
                   "<li class=\"active\" style=\"font-size: 16px;\">" +
             "<a href=\"/Manage/Admin\"><i class=\"fa fa-home\"></i> Dashboard</a> <a href=\"" + urlone + "\"><i class=\"fa fa-angle-right\"></i> " + levelone + "</a> <i class=\"fa fa-angle-right\"></i> " + leveltwo +
             "</li>" +
               "</ol> </div>   </div>";
            }
            else if (!string.IsNullOrEmpty(levelthree))
            {
                html = "  <div class=\"row\">" +
           "<div class=\"col-lg-12\">" +
               "<ol class=\"breadcrumb\">" +
                   "<li class=\"active\" style=\"font-size: 16px;\">" +
             "<a href=\"/Manage/Admin\"><i class=\"fa fa-home\"></i> Dashboard</a> <a href=\"" + urlone + "\"> <i class=\"fa fa-angle-right\"></i> " + levelone + "</a> <i class=\"fa fa-angle-right\"></i> " + leveltwo + "<a href=\"" + urltow + "\"> <i class=\"fa fa-angle-right\"> </i> " + leveltwo + "</a> <i class=\"fa fa-angle-right\"></i> " + levelthree +
             "</li>" +
               "</ol> </div>   </div>";
            }
            return html;
        }
    }
}