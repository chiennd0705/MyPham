using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class MemberProfileDao : AbstractBaseDao<MemberProfile, long>
    {
        public MemberProfileDao()
            : base(typeof(MemberProfile))
        {
        }
    }
}