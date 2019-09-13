using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class UserGroupRoleDao : AbstractBaseDao<UserGroupRole, long>
    {
        public UserGroupRoleDao()
            : base(typeof(UserGroupRole))
        {
        }
    }
}