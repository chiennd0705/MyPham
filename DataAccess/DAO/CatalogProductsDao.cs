using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class CatalogProductsDao : AbstractBaseDao<CatalogProducts, long>
    {
        public CatalogProductsDao()
            : base(typeof(CatalogProducts))
        {
        }
    }
}