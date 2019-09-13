using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class OrderSittingDao : AbstractBaseDao<OrderSitting, long>
    {
        public OrderSittingDao()
            : base(typeof(OrderSitting))
        {
        }
    }
}