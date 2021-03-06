using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class CatalogPropertiesDao : AbstractBaseDao<CatalogProperty, long>
    {
        public CatalogPropertiesDao() : base(typeof(CatalogProperty))
        {
        }
    }
}