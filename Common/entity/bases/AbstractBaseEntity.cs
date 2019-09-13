using System;

namespace Common.entity.bases
{
    [Serializable]
    public abstract class AbstractBaseEntity<TId> where TId : IEquatable<TId>//: INotifyPropertyChanging, INotifyPropertyChanged
    {
        //private Tid id;
        // [Key]
        public virtual TId Id
        {
            get;
            set;
        }
    }
}