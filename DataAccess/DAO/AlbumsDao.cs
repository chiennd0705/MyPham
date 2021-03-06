using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class AlbumsDao : AbstractBaseDao<Album, long>
    {
        public AlbumsDao()
            : base(typeof(Album))
        {
        }
    }
}