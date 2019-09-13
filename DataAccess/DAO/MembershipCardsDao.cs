using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class MembershipCardsDao : AbstractBaseDao<MembershipCard, long>
    {
        public MembershipCardsDao()
            : base(typeof(MembershipCard))
        {
        }
    }
}