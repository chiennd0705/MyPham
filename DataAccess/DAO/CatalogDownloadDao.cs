using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class CatalogDownloadsDao : AbstractBaseDao<CatalogDownloads, long>
    {
        public CatalogDownloadsDao()
            : base(typeof(CatalogDownloads))
        {
        }
    }
}