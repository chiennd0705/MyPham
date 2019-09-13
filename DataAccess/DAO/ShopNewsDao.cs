using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class ShopNewsDao : AbstractBaseDao<ShopNew, long>
    {
        public ShopNewsDao()
            : base(typeof(ShopNew))
        {
        }
    }
}