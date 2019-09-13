using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class ShopNewsGroupsDao : AbstractBaseDao<ShopNewsGroup, long>
    {
        public ShopNewsGroupsDao()
            : base(typeof(ShopNewsGroup))
        {
        }
    }
}