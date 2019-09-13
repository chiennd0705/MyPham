using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class ProductImagesPriceDao : AbstractBaseDao<ProductImagesPrice, long>
    {
        public ProductImagesPriceDao()
            : base(typeof(ProductImagesPrice))
        {
        }
    }
}