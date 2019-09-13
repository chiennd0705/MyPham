using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class PromotionListDao : AbstractBaseDao<PromotionList, long>
    {
        public PromotionListDao()
            : base(typeof(PromotionList))
        {
        }
    }
}