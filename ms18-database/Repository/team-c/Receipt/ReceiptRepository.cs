using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.Services;

namespace Maasgroep.Database.Receipts
{

    public class ReceiptRepository : EditableRepository<Receipt, ReceiptModel, ReceiptHistory>, IReceiptRepository<Receipt, ReceiptHistory>
    {
		public ReceiptRepository(MaasgroepContext db) : base(db) {}

		/** Create ReceiptModel from Receipt record */
		public override ReceiptModel GetModel(Receipt receipt)
        {
			var status = EnumConverterService.ConvertStringToEnum(receipt.ReceiptStatus);
			var costCentre = _db.CostCentre.FirstOrDefault(c => c.Id == receipt.CostCentreId);

            return new ReceiptModel() {
				Id = receipt.Id,
				Note = receipt.Note,
				Amount = receipt.Amount,
				Status = status,
				StatusString = status.ToString(),
				CostCentre = costCentre == null ? null : new() {
					Id = costCentre.Id,
					Name = costCentre.Name,
				},
			};
        }

		/** Create or update Receipt record from model */
        public override Receipt? GetRecord(ReceiptModel model, Receipt? existingReceipt = null)
        {
            var receipt = existingReceipt ?? new();
			if (receipt.ReceiptStatus == ReceiptStatus.Goedgekeurd.ToString()
				|| receipt.ReceiptStatus == ReceiptStatus.Uitbetaald.ToString())
				return null; // Al definitief, dus aanpassen niet meer toegestaan

			var receiptHeeftFotos = _db.Photo.Where(p => p.ReceiptId == model.Id).Count() > 0;
			
			receipt.Note = model.Note;
			receipt.Amount = model.Amount;
			receipt.CostCentreId = model.CostCentre?.Id;
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
			return receipt;
        }

		/** Create a ReceiptHistory record from a Receipt record */ 
        public override ReceiptHistory GetHistory(Receipt receipt)
        {
            return new ReceiptHistory() {
				ReceiptId = receipt.Id,
				Amount = receipt.Amount,
				Note = receipt.Note,
				Location = receipt.Location,
				ReceiptStatus = receipt.ReceiptStatus,
				CostCentreId = receipt.CostCentreId,
			};
        }
    }
}
