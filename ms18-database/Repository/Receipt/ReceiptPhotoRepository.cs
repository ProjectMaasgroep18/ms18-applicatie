using Maasgroep.Database.Admin;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.DataModels.Receipts;

namespace Maasgroep.Database.Receipts
{
    public class ReceiptPhotoRepository : DeletableRepository<ReceiptPhoto, ReceiptPhotoModel, ReceiptPhotoData>, IReceiptPhotoRepository
    {
        protected ReceiptRepository Receipts;
        protected MemberRepository Members;
		public ReceiptPhotoRepository(MaasgroepContext db) : base(db)
        {
            Receipts = new(db);
            Members = new(db);
        }

        /** Create ReceiptPhotoModel from ReceiptPhoto record */
        public override ReceiptPhotoModel GetModel(ReceiptPhoto photo)
        {
            var member = photo.MemberCreatedId != null ? Members.GetModel((long)photo.MemberCreatedId) : null;

            return new ReceiptPhotoModel() {
                Id = photo.Id,
				FileExtension = photo.FileExtension,
				FileName = photo.FileName,
                ReceiptId = photo.ReceiptId,
				Base64Image = photo.Base64Image,
                MemberCreated = member,
			};
        }

        /** Create or update ReceiptPhoto record from data model */
        public override ReceiptPhoto? GetRecord(ReceiptPhotoData data, ReceiptPhoto? existingPhoto = null)
        {
            if (existingPhoto != null)
                return null; // Photo records are not editable

            if (!Receipts.Exists(data.ReceiptId))
                return null; // Receipt not found

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

        /** Make sure receipt status is updated (Concept / Ingediend) if related photo is added or deleted */
        public override Action GetAfterSaveAction(ReceiptPhoto record)
        {
            var afterSaveAction = base.GetAfterSaveAction(record);
            return () => {
                afterSaveAction.Invoke();
                var receipt = Receipts.GetById(record.ReceiptId);
                if (receipt == null)
                    return;
                var currentStatus = receipt.ReceiptStatus;
                Console.WriteLine(currentStatus);
                receipt = Receipts.GetRecord(new() { Amount = receipt.Amount, Note = receipt.Note, CostCentreId = receipt.CostCentreId }, receipt);
                Console.WriteLine(receipt?.ReceiptStatus);
                if (receipt != null && receipt.ReceiptStatus != currentStatus) {
                    // Status changed (because now complete or no longer so in case of delete)
                    Console.WriteLine($"RECEIPT STATUS CHANGE IN {receipt.Id}: {currentStatus} > {receipt.ReceiptStatus}");
                    Db.Receipt.Update(receipt);
                    Db.SaveChanges();
                }
            };
        }
    }
}
