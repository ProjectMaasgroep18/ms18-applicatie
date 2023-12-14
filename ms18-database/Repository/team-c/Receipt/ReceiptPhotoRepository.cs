using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.DataModels.Receipts;

namespace Maasgroep.Database.Receipts
{

    public class ReceiptPhotoRepository : DeletableRepository<ReceiptPhoto, ReceiptPhotoModel, ReceiptPhotoData>, IReceiptPhotoRepository
    {
		public ReceiptPhotoRepository(MaasgroepContext db) : base(db) {}

        /** Create ReceiptPhotoModel from ReceiptPhoto record */
        public override ReceiptPhotoModel GetModel(ReceiptPhoto photo)
        {
            var member = Db.Member.FirstOrDefault(c => c.Id == photo.MemberCreatedId);

            return new ReceiptPhotoModel() {
				FileExtension = photo.FileExtension,
				FileName = photo.FileName,
                ReceiptId = photo.ReceiptId,
				Base64Image = photo.Base64Image,
                MemberCreated = member == null ? null : new() {
					Id = member.Id,
					Name = member.Name,
				},
			};
        }

        /** Create or update ReceiptPhoto record from data model */
        public override ReceiptPhoto? GetRecord(ReceiptPhotoData data, ReceiptPhoto? existingPhoto = null)
        {
            if (existingPhoto != null)
                return null; // Photo records are not editable

            var photo = new ReceiptPhoto() {
                Base64Image = data.Base64Image,
				FileExtension = data.FileExtension,
				FileName = data.FileName,
				ReceiptId = data.ReceiptId,
            };
			return photo;
        }

        /** List photos by receipt */
		public IEnumerable<ReceiptPhotoModel> ListByReceipt(long receiptId, int offset = default, int limit = default, bool includeDeleted = default)
			=> GetList(item => item.ReceiptId == receiptId, null, offset, limit, includeDeleted).Select(item => GetModel(item)!);
    }
}
