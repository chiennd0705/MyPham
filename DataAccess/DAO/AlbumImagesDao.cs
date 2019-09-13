using Common;
using DataAccess.bases;

namespace DataAccess.DAO
{
    public class AlbumImagesDao : AbstractBaseDao<AlbumImage, long>
    {
        public AlbumImagesDao()
            : base(typeof(AlbumImage))
        {
        }
    }
}