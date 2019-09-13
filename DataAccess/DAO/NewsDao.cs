using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class NewsDao : AbstractBaseDao<News, long>
    {
        public NewsDao()
            : base(typeof(News))
        {
        }
    }
}