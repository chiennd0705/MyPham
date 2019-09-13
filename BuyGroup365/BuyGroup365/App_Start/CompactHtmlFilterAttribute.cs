using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

public class CompactHtmlFilterAttribute : ActionFilterAttribute
{
    public class WhitespaceFilter : MemoryStream
    {
        private string Source = string.Empty;
        private Stream Filter = null;

        public WhitespaceFilter(HttpResponseBase HttpResponseBase)
        {
            Filter = HttpResponseBase.Filter;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Source = UTF8Encoding.UTF8.GetString(buffer).Replace("\r", "").Replace("\n", "").Replace("\t", "");
            Filter.Write(UTF8Encoding.UTF8.GetBytes(Source), offset, UTF8Encoding.UTF8.GetByteCount(Source));
        }
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
#if DEBUG
        base.OnActionExecuting(filterContext);
#else
                        try
                        {
                            filterContext.HttpContext.Response.Filter = new WhitespaceFilter(filterContext.HttpContext.Response);
                        }
                        catch (Exception) { }
#endif
    }
}