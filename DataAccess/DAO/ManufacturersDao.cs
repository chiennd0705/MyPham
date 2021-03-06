using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class ManufacturersDao : AbstractBaseDao<Manufacturer, long>
    {
        public ManufacturersDao()
            : base(typeof(Manufacturer))
        {
        }
    }
}