using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.Database.Receipts
{

    public class ReceiptPhotoRepository : DeletableRepository<Photo, PhotoModel>, IReceiptPhotoRepository<Photo>
    {
		public ReceiptPhotoRepository(MaasgroepContext db) : base(db) {}

        /** Create ReceiptPhotoModel from ReceiptPhoto record */
        public override PhotoModel GetModel(Photo photo)
        {
            return new PhotoModel() {
				FileExtension = photo.FileExtension,
				FileName = photo.FileName,
                ReceiptId = photo.ReceiptId,
				Base64Image = photo.Base64Image,
			};
        }

        /** Create or update ReceiptPhoto record from model */
        public override Photo? GetRecord(PhotoModel model, Photo? existingPhoto = null)
        {
            if (existingPhoto != null)
                return null; // Photo records are not editable

            var photo = new Photo() {
                Base64Image = model.Base64Image,
				FileExtension = model.FileExtension,
				FileName = model.FileName,
				ReceiptId = model.ReceiptId,
            };
			return photo;
        }
    }
}
