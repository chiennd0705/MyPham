using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class ProductAttributeDao : AbstractBaseDao<ProductAttribute, long>
    {
        public ProductAttributeDao()
            : base(typeof(ProductAttribute))
        {
        }
    }
}