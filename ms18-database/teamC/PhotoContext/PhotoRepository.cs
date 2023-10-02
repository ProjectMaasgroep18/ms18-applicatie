
namespace Maasgroep.Database.Photos
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly PhotoContext _db;
        public PhotoRepository() 
        { 
            _db = new PhotoContext();
        }

        public void AddPhoto()
        {
            throw new NotImplementedException();
        }

        public void RemovePhoto()
        {
            throw new NotImplementedException();
        }
    }
}
