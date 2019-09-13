using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class PayFormDao : AbstractBaseDao<PayForm, long>
    {
        public PayFormDao()
            : base(typeof(PayForm))
        {
        }
    }
}