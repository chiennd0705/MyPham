using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class NewsGroupDao : AbstractBaseDao<NewsGroups, long>
    {
        public NewsGroupDao()
            : base(typeof(NewsGroups))
        {
        }
    }
}