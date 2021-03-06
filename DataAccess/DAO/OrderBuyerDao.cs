using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class OrderBuyerDao : AbstractBaseDao<OrderBuyer, long>
    {
        public OrderBuyerDao()
            : base(typeof(OrderBuyer))
        {
        }
    }
}