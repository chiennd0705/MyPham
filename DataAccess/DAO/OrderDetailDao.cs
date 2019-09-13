using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class OrderDetailDao : AbstractBaseDao<OrderDetail, long>
    {
        public OrderDetailDao()
            : base(typeof(OrderDetail))
        {
        }
    }
}