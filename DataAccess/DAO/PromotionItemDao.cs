using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class PromotionItemDao : AbstractBaseDao<PromotionItem, long>
    {
        public PromotionItemDao()
            : base(typeof(PromotionItem))
        {
        }
    }
}