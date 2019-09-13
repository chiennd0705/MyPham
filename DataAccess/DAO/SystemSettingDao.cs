using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class SystemSettingDao : AbstractBaseDao<SystemSetting, long>
    {
        public SystemSettingDao()
            : base(typeof(SystemSetting))
        {
        }
    }
}