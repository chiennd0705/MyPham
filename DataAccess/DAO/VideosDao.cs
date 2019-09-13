using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class VideosDao : AbstractBaseDao<Videos, long>
    {
        public VideosDao()
            : base(typeof(Videos))
        {
        }
    }
}