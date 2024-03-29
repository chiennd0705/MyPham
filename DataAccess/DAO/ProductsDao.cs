using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class ProductsDao : AbstractBaseDao<Product, long>
    {
        public ProductsDao()
            : base(typeof(Product))
        {
        }
    }
}