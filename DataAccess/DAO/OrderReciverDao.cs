using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class OrderReciverDao : AbstractBaseDao<OrderReciver, long>
    {
        public OrderReciverDao()
            : base(typeof(OrderReciver))
        {
        }
    }
}