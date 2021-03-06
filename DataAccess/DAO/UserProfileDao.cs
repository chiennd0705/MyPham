using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class UserProfileDao : AbstractBaseDao<UserProfile, long>
    {
        public UserProfileDao()
            : base(typeof(UserProfile))
        {
        }
    }
}