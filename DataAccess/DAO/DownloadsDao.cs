using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class DownloadsDao : AbstractBaseDao<Downloads, long>
    {
        public DownloadsDao()
            : base(typeof(Downloads))
        {
        }
    }
}