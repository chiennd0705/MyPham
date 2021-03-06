using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class Catalogs_ManufacturersDao : AbstractBaseDao<Catalogs_Manufacturers, long>
    {
        public Catalogs_ManufacturersDao()
            : base(typeof(Catalogs_Manufacturers))
        {
        }
    }
}