using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.Services;
using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database.Receipts
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly MaasgroepContext _db;
        public ReceiptRepository(MaasgroepContext db) 
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
	}
}
