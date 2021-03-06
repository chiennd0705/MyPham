using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class FriendlyUrlDao : AbstractBaseDao<FriendlyUrl, long>
    {
        public FriendlyUrlDao()
            : base(typeof(FriendlyUrl))
        {
        }
    }
}