using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class TextHtmlDao : AbstractBaseDao<TextHtml, long>
    {
        public TextHtmlDao()
            : base(typeof(TextHtml))
        {
        }
    }
}