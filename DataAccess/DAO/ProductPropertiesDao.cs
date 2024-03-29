using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class ProductPropertiesDao : AbstractBaseDao<ProductProperty, long>
    {
        public ProductPropertiesDao()
            : base(typeof(ProductProperty))
        {
        }
    }
}