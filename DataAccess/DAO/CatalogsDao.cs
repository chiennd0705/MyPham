using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class CatalogsDao : AbstractBaseDao<Catalog, long>
    {
        public CatalogsDao()
            : base(typeof(Catalog))
        {
        }
    }
}