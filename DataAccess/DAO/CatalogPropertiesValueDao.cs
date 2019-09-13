using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class CatalogPropertiesValueDao : AbstractBaseDao<CatalogPropertiesValue, long>
    {
        public CatalogPropertiesValueDao()
            : base(typeof(CatalogPropertiesValue))
        {
        }
    }
}