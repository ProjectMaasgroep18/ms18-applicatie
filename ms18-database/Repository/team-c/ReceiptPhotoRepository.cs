using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.Services;

namespace Maasgroep.Database.Receipts
{

    public class ReceiptPhotoRepository : GenericRepository<Photo, PhotoModel, PhotoHistory>, IReceiptPhotoRepository<Photo, PhotoHistory>
    {
		public ReceiptPhotoRepository(MaasgroepContext db) : base(db) {}

        public override PhotoModel GetModel(Photo record)
        {
            throw new NotImplementedException();
        }

        public override Photo? GetRecord(PhotoModel model, Photo? existingRecord = null)
        {
            throw new NotImplementedException();
        }
        
        public override PhotoHistory GetHistory(Photo record)
        {
            throw new NotImplementedException();
        }
    }
}
