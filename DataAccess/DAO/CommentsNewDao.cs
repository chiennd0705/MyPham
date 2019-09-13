using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class CommentsNewDao : AbstractBaseDao<CommentsNew, long>
    {
        public CommentsNewDao()
            : base(typeof(CommentsNew))
        {
        }
    }
}