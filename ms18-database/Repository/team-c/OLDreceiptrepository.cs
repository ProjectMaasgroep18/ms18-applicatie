using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.Services;
using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database.Receipts
{

	//TODO: deze gaat ook teveel doen (receipt + photo + costcentre).
    public class OLDReceiptRepository : IReceiptRepository
    {
        private readonly MaasgroepContext _db;
        public OLDReceiptRepository(MaasgroepContext db) 
        {
			_db = db;
        }


		#region Photo
		public long Add(PhotoModelCreateDb photo)
		{
			var photoToAdd = new Photo()
			{
				Base64Image = photo.PhotoModel.Base64Image,
				fileExtension = photo.PhotoModel.FileExtension,
				fileName = photo.PhotoModel.FileName,
				ReceiptId = photo.ReceiptId,
				MemberCreatedId = photo.Member.Id
			};

			_db.Database.BeginTransaction();
			_db.Photo.Add(photoToAdd);
			_db.SaveChanges();

			var idOfPhoto = _db.Database.SqlQuery<long>($"select currval('receipt.\"photoSeq\"'::regclass)").ToList().FirstOrDefault();
			_db.Database.CommitTransaction();

			return idOfPhoto;
		}

		public PhotoModel GetPhoto(long id) //TODO: Dit was PhotoViewModel; ik denk dat we funcitoneel niet 1 foto willen teruggeven?
		{
			var result = new PhotoModel();

			var photo = _db.Photo.Where(p => p.Id == id).FirstOrDefault();

			if (photo == null)
			{
				throw new Exception("kapot");
			}
			else
			{
				result.fileExtension = photo.fileExtension;
				result.fileName = photo.fileName;
				result.Base64Image = photo.Base64Image;
			}

			return result;
		}

		public IEnumerable<PhotoModel> GetPhotosByReceipt(long receiptId, int offset = 0, int limit = 100, bool includeDeleted = false)
		{
			var result = new List<PhotoModel>();

			var dbResults = _db.Photo
				.Where(p => p.ReceiptId == receiptId && (p.DateTimeDeleted == null || includeDeleted))
				.Skip(offset)
				.Take(limit)
				.ToList();

			foreach (var dbResult in dbResults)
				result.Add(GetPhoto(dbResult.Id));

			return result;
		}

		public bool Delete(PhotoModelDeleteDb photoToDelete)
		{
			var photo = _db.Photo.Where(p => p.Id == photoToDelete.Photo.Id).FirstOrDefault();

			if (photo != null)
			{
				var history = CreatePhotoHistory(photo);

				_db.Database.BeginTransaction();
				_db.PhotoHistory.Add(history);
				_db.SaveChanges();

				photo.DateTimeDeleted = DateTime.UtcNow;
				photo.MemberDeletedId = photoToDelete.Member.Id;

				_db.Photo.Update(photo);
				_db.SaveChanges();
				_db.Database.CommitTransaction(); //TODO: Misschien nog iets met finally en rollback enzo.
			}
			else
			{
				return false; //TODO: In de aanroep iets mee doen
			}

			return true;
		}

		#endregion

		#region Approval
		public bool AddApproval(ReceiptApprovalModelCreateDb approval)
		{
			var approvalToAdd = new ReceiptApproval()
			{ 
				Approved = approval.Approval.Approved,
				Note = approval.Approval.Note,
				ReceiptId = approval.Approval.ReceiptId,
				MemberCreatedId = approval.Member.Id
			};

			_db.ReceiptApproval.Add(approvalToAdd);
			_db.SaveChanges();

			return true;
		}
		#endregion


		private PhotoHistory CreatePhotoHistory(Photo photo)
		{
			var history = new PhotoHistory();

			history.PhotoId = photo.Id;
			history.ReceiptId = photo.ReceiptId;
			history.fileExtension = photo.fileExtension;
			history.fileName = photo.fileName;
			history.Location = photo.Location;
			//Geen base64 history.
			history.MemberCreatedId = photo.MemberCreatedId;
			history.MemberModifiedId = photo.MemberModifiedId;
			history.MemberDeletedId = photo.MemberDeletedId;
			history.DateTimeCreated = photo.DateTimeCreated;
			history.DateTimeModified = photo.DateTimeModified;
			history.DateTimeDeleted = photo.DateTimeDeleted;

			return history;
		}

		private ReceiptApprovalHistory CreateReceiptApprovalHistory(ReceiptApproval approval)
		{
			var history = new ReceiptApprovalHistory();

			history.ReceiptId = approval.ReceiptId;
			history.Approved = approval.Approved;
			history.Note = approval.Note;

			history.MemberCreatedId = approval.MemberCreatedId;
			history.MemberModifiedId = approval.MemberModifiedId;
			history.MemberDeletedId = approval.MemberDeletedId;
			history.DateTimeCreated = approval.DateTimeCreated;
			history.DateTimeModified = approval.DateTimeModified;
			history.DateTimeDeleted = approval.DateTimeDeleted;

			return history;
		}
	}
}
