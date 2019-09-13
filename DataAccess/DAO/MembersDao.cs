using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class MembersDao : AbstractBaseDao<Member, long>
    {
        public MembersDao()
            : base(typeof(Member))
        {
        }
    }
}