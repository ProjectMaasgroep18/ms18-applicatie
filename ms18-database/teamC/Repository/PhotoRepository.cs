
namespace Maasgroep.Database.Photos
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly MaasgroepContext _db;
        public PhotoRepository() 
        { 
            _db = new MaasgroepContext();
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
