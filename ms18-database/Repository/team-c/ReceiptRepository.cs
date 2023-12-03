using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.Services;
using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database.Receipts
{

	//TODO: deze gaat ook teveel doen (receipt + photo + costcentre).
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly ReceiptContext _db;
        public ReceiptRepository(ReceiptContext db) 
        {
			_db = db;
        }
		public long AddReceipt(ReceiptModelCreateDb receipt)
		{
			var costCentre = _db.CostCentre.Where(c => c.Name == receipt.ReceiptModel.CostCentre).FirstOrDefault();
			if (costCentre == null) throw new Exception("kapot!");

			var receiptToAdd = new Receipt()
			{
				Amount = receipt.ReceiptModel.Amount,
				Note = receipt.ReceiptModel.Note,
				CostCentreId = costCentre.Id,
				ReceiptStatus = "Ingediend",
				MemberCreatedId = receipt.Member.Id
			};

			_db.Database.BeginTransaction();
			_db.Receipt.Add(receiptToAdd);
			_db.SaveChanges();
			
			var idOfReceipt = _db.Database.SqlQuery<long>($"select currval('receipt.\"receiptSeq\"'::regclass)").ToList().FirstOrDefault();
			
			//TODO: Dit staat nu ook dubbel met photo toevoegen.
			if (receipt.ReceiptModel.Photos != null && receipt.ReceiptModel.Photos.Count > 0) 
			{
				foreach (var photo in receipt.ReceiptModel.Photos)
					_db.Photo.Add(new Photo()
					{ 
						Base64Image = photo.Base64Image,
						fileExtension = photo.FileExtension,
						fileName = photo.FileName,
						ReceiptId = idOfReceipt,
						MemberCreatedId = receipt.Member.Id
					});
			}

			_db.SaveChanges();

			_db.Database.CommitTransaction();

			return idOfReceipt;

		}

		public bool DeleteReceipt(ReceiptModel receipt)
		{
			throw new NotImplementedException();
		}

		public ReceiptModel GetReceipt(long id)
		{
			ReceiptModel result = new ReceiptModel();

			var receipt = _db.Receipt.FirstOrDefault(r => r.Id == id);
			if (receipt == null) 
			{
				throw new InvalidOperationException();
			}
			else
			{
				var costCentre = from r in _db.Receipt
								 join c in _db.CostCentre
								 on r.CostCentreId equals c.Id
								 into leftJoin
								 from rc in leftJoin.DefaultIfEmpty()
								 where r.Id == id
								 select new { r, rc };

				var photos = from p in _db.Photo
							 join c in costCentre
							 on p.ReceiptId equals c.r.Id
							 where c.r.Id == id
							 select new { p, c };

				result.Id = costCentre.FirstOrDefault()!.r.Id;
				result.Note = costCentre.FirstOrDefault()!.r.Note;
				result.Status = EnumConverterService.ConvertStringToEnum(costCentre.FirstOrDefault()!.r.ReceiptStatus);
				result.StatusString = costCentre.FirstOrDefault()!.r.ReceiptStatus;
				result.Amount = costCentre.FirstOrDefault()!.r.Amount;
				result.CostCentre.Id = costCentre.FirstOrDefault()!.rc.Id;
				result.CostCentre.Name = costCentre.FirstOrDefault()!.rc.Name;
				foreach (var photo in photos)
					result.Photos.Add(new PhotoModel() 
					{ 
						Base64Image = photo.p.Base64Image
					,	fileExtension = photo.p.fileExtension
					,	fileName = photo.p.fileName
					,	Id = photo.p.Id
					});
			}

			return result;
		}

		public IEnumerable<ReceiptModel> GetReceipts(int offset, int fetch)
		{
			var result = new List<ReceiptModel>();

			var indexStart = new Index(offset);
			var indexEnd = new Index(fetch);
			var range = new Range(indexStart, indexEnd);
			//var dbResults = _db.Receipt.Take(range).ToList();
			var dbResults = _db.Receipt.ToList();

			foreach (var dbResult in dbResults)
				result.Add(GetReceipt(dbResult.Id));

			return result;
		}

		public IEnumerable<ReceiptModel> GetReceiptsByMember(long memberId, int offset, int fetch)
		{
			var result = new List<ReceiptModel>();

			var indexStart = new Index(offset);
			var indexEnd = new Index(fetch);
			var range = new Range(indexStart, indexEnd);
			//var dbResults = _db.Receipt.Take(range).ToList(); TODO: hiero offset fixen
			var dbResults = _db.Receipt.Where(r => r.MemberCreatedId == memberId).ToList();

			foreach (var dbResult in dbResults)
				result.Add(GetReceipt(dbResult.Id));

			return result;
		}

		public bool ModifyReceipt(ReceiptModelUpdateDb receiptUpdated)
		{
			var receipt = _db.Receipt.Where(r => r.Id == receiptUpdated.ReceiptModel.Id).FirstOrDefault();

			if (receipt == null) throw new Exception("kapot!");

			_db.ReceiptHistory.Add(CreateReceiptHistory(receipt));

			receipt.Note = receiptUpdated.ReceiptModel.Note;
			receipt.Amount = receiptUpdated.ReceiptModel.Amount;
			receipt.CostCentreId = receiptUpdated.ReceiptModel.CostCentre.Id;
			receipt.MemberModifiedId = receiptUpdated.Member.Id;
			receipt.DateTimeModified = DateTime.UtcNow;
			receipt.ReceiptStatus = receiptUpdated.ReceiptModel.StatusString!;

			_db.Update(receipt);
			_db.SaveChanges();

			return true;
		}

		public long AddPhoto(PhotoModelCreateDb photo)
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

		public IEnumerable<PhotoModel> GetPhotosByReceipt(long receiptId, int offset, int fetch)
		{
			var result = new List<PhotoModel>();

			var indexStart = new Index(offset);
			var indexEnd = new Index(fetch);
			var range = new Range(indexStart, indexEnd);
			//var dbResults = _db.Receipt.Take(range).ToList(); TODO: hiero offset fixen
			var dbResults = _db.Photo.Where(p => p.ReceiptId == receiptId).ToList();

			foreach (var dbResult in dbResults)
				result.Add(GetPhoto(dbResult.Id));

			return result;
		}

		public bool DeletePhoto(long id)
		{
			var photoToDelete = _db.Photo.Where(p => p.Id == id).FirstOrDefault();

			if (photoToDelete != null)
			{
				var history = CreatePhotoHistory(photoToDelete);

				_db.Database.BeginTransaction();
				_db.PhotoHistory.Add(history);
				_db.Photo.Remove(photoToDelete);
				_db.SaveChanges();
				_db.Database.RollbackTransaction(); //TODO: Misschien nog iets met finally en rollback enzo.
			}
			else
			{
				return false; //TODO: In de aanroep iets mee doen
			}

			return true;
		}

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

		private ReceiptHistory CreateReceiptHistory(Receipt receipt)
		{
			ReceiptHistory history = new ReceiptHistory();

			history.Id = receipt.Id;
			history.Amount = receipt.Amount;
			history.Note = receipt.Note;
			history.Location = receipt.Location;
			history.ReceiptStatus = receipt.ReceiptStatus;
			history.CostCentreId = receipt.CostCentreId;
			history.MemberCreatedId = receipt.MemberCreatedId;
			history.MemberModifiedId = receipt.MemberModifiedId;
			history.MemberDeletedId = receipt.MemberDeletedId;
			history.DateTimeCreated = receipt.DateTimeCreated;
			history.DateTimeModified = receipt.DateTimeModified;
			history.DateTimeDeleted = receipt.DateTimeDeleted;

			return history;
		}

		private PhotoHistory CreatePhotoHistory(Photo photo)
		{ 
			PhotoHistory history = new PhotoHistory();

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

		public IEnumerable<CostCentreModel> GetCostCentres(int offset, int fetch)
		{
			throw new NotImplementedException();
		}

		public CostCentreModel GetCostCentre(long id)
		{
			throw new NotImplementedException();
		}

		public long AddCostCentre(CostCentreModelCreateDb costCentre)
		{
			throw new NotImplementedException();
		}

		public bool ModifyCostCentre(CostCentreModelUpdateDb costCentre)
		{
			throw new NotImplementedException();
		}

		public bool DeleteCostCentre(long id)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<CostCentreModel> GetReceiptsByCostCentre(long costCentreId, int offset, int fetch)
		{
			throw new NotImplementedException();
		}
	}
}
