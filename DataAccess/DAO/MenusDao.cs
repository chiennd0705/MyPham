using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class MenusDao : AbstractBaseDao<Menus, long>
    {
        public MenusDao()
            : base(typeof(Menus))
        {
        }
    }
}