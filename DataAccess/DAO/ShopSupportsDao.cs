using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class ShopSupportsDao : AbstractBaseDao<ShopSupport, long>
    {
        public ShopSupportsDao()
            : base(typeof(ShopSupport))
        {
        }
    }
}