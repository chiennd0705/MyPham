using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class UserDao : AbstractBaseDao<User, long>
    {
        public UserDao()
            : base(typeof(User))
        {
        }
    }
}