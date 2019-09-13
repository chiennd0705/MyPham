using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class ProductImagesDao : AbstractBaseDao<ProductImage, long>
    {
        public ProductImagesDao()
            : base(typeof(ProductImage))
        {
        }
    }
}