using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class RoleDao : AbstractBaseDao<Role, long>
    {
        public RoleDao()
            : base(typeof(Role))
        {
        }
    }
}