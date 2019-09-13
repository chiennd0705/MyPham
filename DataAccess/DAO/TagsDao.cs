using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class TagsDao : AbstractBaseDao<Tag, long>
    {
        public TagsDao()
            : base(typeof(Tag))
        {
        }
    }
}