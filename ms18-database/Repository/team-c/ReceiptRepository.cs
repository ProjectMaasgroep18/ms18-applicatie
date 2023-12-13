using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.Services;
using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database.Receipts
{

	//TODO: deze gaat ook teveel doen (receipt + photo + costcentre).
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly MaasgroepContext _db;
        public ReceiptRepository(MaasgroepContext db) 
        {
			_db = db;
        }

		#region Receipt

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
		public IEnumerable<ReceiptModel> GetReceipts(int offset = 0, int limit = 100, bool includeDeleted = false)
		{
			var result = new List<ReceiptModel>();

			var dbResults = _db.Receipt
				.Where(r => r.DateTimeDeleted == null || includeDeleted)
				.Skip(offset)
				.Take(limit)
				.ToList();

			foreach (var dbResult in dbResults)
				result.Add(GetReceipt(dbResult.Id));

			return result;
		}
		public IEnumerable<ReceiptModel> GetReceiptsByMember(long memberId, int offset = 0, int limit = 100, bool includeDeleted = false)
		{
			var result = new List<ReceiptModel>();

			var indexStart = new Index(offset);
			var indexEnd = new Index(limit);
			var range = new Range(indexStart, indexEnd);
			var dbResults = _db.Receipt
				.Where(r => r.MemberCreatedId == memberId && (r.DateTimeDeleted == null || includeDeleted))
				.Skip(offset)
				.Take(limit)
				.ToList();

			foreach (var dbResult in dbResults)
				result.Add(GetReceipt(dbResult.Id));

			return result;
		}
		public long Add(ReceiptModelCreateDb receipt)
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
		public bool Modify(ReceiptModelUpdateDb receiptUpdated)
		{
			var receipt = _db.Receipt.Where(r => r.Id == receiptUpdated.ReceiptModel.Id).FirstOrDefault();

			if (receipt == null) throw new Exception("Receipt niet gevonden!");

			if (receipt.ReceiptStatus == ReceiptStatus.Goedgekeurd.ToString()
				|| receipt.ReceiptStatus == ReceiptStatus.Uitbetaald.ToString()) {
				throw new Exception("Deze receipt mag niet meer aangepast worden!");
			}

			var receiptHeeftFotos = _db.Photo.Where(p => p.ReceiptId == receiptUpdated.ReceiptModel.Id).Count() > 0;

			_db.Database.BeginTransaction();
			_db.ReceiptHistory.Add(CreateReceiptHistory(receipt));
			_db.SaveChanges();

			receipt.Note = receiptUpdated.ReceiptModel.Note;
			receipt.Amount = receiptUpdated.ReceiptModel.Amount;
			receipt.CostCentreId = receiptUpdated.ReceiptModel.CostCentre?.Id;
			receipt.MemberModifiedId = receiptUpdated.Member.Id;
			receipt.DateTimeModified = DateTime.UtcNow;
			if (receiptHeeftFotos
				&& receipt.Note != null
				&& receipt.Note?.Trim() != ""
				&& receipt.Amount != null
				&& receipt.Amount != 0
				&& receipt.CostCentreId != null) {
				// We hebben alle benodigde gegevens
				receipt.ReceiptStatus = ReceiptStatus.Ingediend.ToString();
			} else {
				// We hebben niet alle benodigde gegevens
				receipt.ReceiptStatus = ReceiptStatus.Concept.ToString();
			}

			_db.Update(receipt);
			_db.SaveChanges();
			_db.Database.CommitTransaction();

			return true;
		}
		public bool Delete(ReceiptModelDeleteDb receiptToDelete)
		{
			var receipt = _db.Receipt.Where(cc => cc.Id == receiptToDelete.Receipt.Id).FirstOrDefault();

			if (receipt != null)
			{
				var history = CreateReceiptHistory(receipt);

				_db.Database.BeginTransaction();
				_db.ReceiptHistory.Add(history);
				_db.SaveChanges();

				receipt.MemberDeletedId = receiptToDelete.Member.Id;
				receipt.DateTimeDeleted = DateTime.UtcNow;

				_db.Receipt.Update(receipt);
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

		#region CostCentre
		public IEnumerable<CostCentreModel> GetCostCentres(int offset = 0, int limit = 100, bool includeDeleted = false)
		{
			var result = new List<CostCentreModel>();

			var allCostCentre = _db.CostCentre.ToList();
			foreach (var costCentre in allCostCentre)
				result.Add(GetCostCentre(costCentre.Id));

			return result;
		}

		public CostCentreModel GetCostCentre(long id)
		{
			CostCentreModel result = new CostCentreModel();

			var costCentre = _db.CostCentre.Where(c => c.Id == id).FirstOrDefault();

			if (costCentre == null)
			{
				throw new InvalidOperationException();
			}
			else
			{
				result.Name = costCentre.Name;
				result.Id = costCentre.Id;
			}

			return result;
		}

		public long Add(CostCentreModelCreateDb costCentre)
		{
			var costCentreToAdd = new CostCentre()
			{
				Name = costCentre.CostCentre.Name,
				MemberCreatedId = costCentre.Member.Id,
				DateTimeCreated = DateTime.UtcNow
			};

			_db.Database.BeginTransaction();

			_db.CostCentre.Add(costCentreToAdd);
			_db.SaveChanges();

			var idOfCostCentre = _db.Database.SqlQuery<long>($"select currval('receipt.\"costCentreSeq\"'::regclass)").ToList().FirstOrDefault();

			_db.Database.CommitTransaction();
			
			return idOfCostCentre;
		}

		public bool Modify(CostCentreModelUpdateDb costCentreUpdated)
		{
			var costCentre = _db.CostCentre.Where(c => c.Id == costCentreUpdated.CostCentre.Id).FirstOrDefault();

			if (costCentre == null) throw new Exception("kapot!");

			_db.Database.BeginTransaction();
			_db.CostCentreHistory.Add(CreateCostCentreHistory(costCentre));
			_db.SaveChanges();

			costCentre.Name = costCentreUpdated.CostCentre.Name;
			costCentre.MemberModifiedId = costCentreUpdated.Member.Id;
			costCentre.DateTimeModified = DateTime.UtcNow;

			_db.Update(costCentre);
			_db.SaveChanges();

			_db.Database.CommitTransaction();

			return true;
		}

		public bool Delete(CostCentreModelDeleteDb costCentreDeleted)
		{
			var costCentre = _db.CostCentre.Where(cc => cc.Id == costCentreDeleted.CostCentre.Id).FirstOrDefault();

			if (costCentre != null)
			{
				var history = CreateCostCentreHistory(costCentre);

				_db.Database.BeginTransaction();
				_db.CostCentreHistory.Add(history);
				_db.SaveChanges();

				costCentre.MemberDeletedId = costCentreDeleted.Member.Id;
				costCentre.DateTimeDeleted = DateTime.UtcNow;

				_db.CostCentre.Update(costCentre);
				_db.SaveChanges();
				_db.Database.CommitTransaction(); //TODO: Misschien nog iets met finally en rollback enzo.
			}
			else
			{
				return false; //TODO: In de aanroep iets mee doen
			}

			return true;
		}

		public IEnumerable<ReceiptModel> GetReceiptsByCostCentre(long costCentreId, int offset = 0, int limit = 100, bool includeDeleted = false)
		{
			var result = new List<ReceiptModel>();

			var receiptsByCostCentre = (from r in _db.Receipt
									   join c in _db.CostCentre
									   on r.CostCentreId equals c.Id
									   where c.Id == costCentreId && (r.DateTimeDeleted == null || includeDeleted)
									   select new { ReceiptjeIdtje = r.Id }).Skip(offset).Take(limit);

			foreach (var item in receiptsByCostCentre)
				result.Add(GetReceipt(item.ReceiptjeIdtje));

			return result;

		}

		#endregion

		private ReceiptHistory CreateReceiptHistory(Receipt receipt)
		{
			var history = new ReceiptHistory();

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
		private CostCentreHistory CreateCostCentreHistory(CostCentre costCentre)
		{
			var history = new CostCentreHistory();

			history.CostCentreId = costCentre.Id;
			history.Name = costCentre.Name;

			history.MemberCreatedId = costCentre.MemberCreatedId;
			history.MemberModifiedId = costCentre.MemberModifiedId;
			history.MemberDeletedId = costCentre.MemberDeletedId;
			history.DateTimeCreated = costCentre.DateTimeCreated;
			history.DateTimeModified = costCentre.DateTimeModified;
			history.DateTimeDeleted = costCentre.DateTimeDeleted;

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
