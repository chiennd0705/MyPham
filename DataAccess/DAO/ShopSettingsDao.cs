using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class ShopSettingsDao : AbstractBaseDao<ShopSetting, long>
    {
        public ShopSettingsDao()
            : base(typeof(ShopSetting))
        {
        }
    }
}