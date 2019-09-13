using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class ConfigRedirectUrlDao : AbstractBaseDao<ConfigRedirectUrl, long>
    {
        public ConfigRedirectUrlDao()
            : base(typeof(ConfigRedirectUrl))
        {
        }
    }
}