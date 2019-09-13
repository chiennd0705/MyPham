using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class MemberCareProductDao : AbstractBaseDao<MemberCareProduct, long>
    {
        public MemberCareProductDao()
            : base(typeof(MemberCareProduct))
        {
        }
    }
}