using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class UserUsergroupDao : AbstractBaseDao<UserUsergroup, long>
    {
        public UserUsergroupDao()
            : base(typeof(UserUsergroup))
        {
        }
    }
}