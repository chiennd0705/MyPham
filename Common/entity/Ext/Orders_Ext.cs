using Common.entity.bases;
using System;

namespace Common
{
    [Serializable]
    public partial class Order : AbstractBaseEntity<long>
    {
        public Shop ShopOrder { get; set; }
    }
}