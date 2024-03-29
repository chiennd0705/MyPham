using Common.entity.bases;
using System;

namespace Common
{
    [Serializable]
    public partial class Shop : AbstractBaseEntity<long>
    {
        public string Email { set; get; }
        public string Skype { set; get; }
        public string Facebook { set; get; }
        public int NumberOfOrder { set; get; }
        public int NumberOfProduct { set; get; }
    }
}