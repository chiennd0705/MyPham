using Common.entity.bases;
using System;

namespace Common
{
    [Serializable]
    public partial class Catalog : AbstractBaseEntity<long>
    {
        public string Stt { get; set; }
        public string NameChild { get; set; }
    }
}