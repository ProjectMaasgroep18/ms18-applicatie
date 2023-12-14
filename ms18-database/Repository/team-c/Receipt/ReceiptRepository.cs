using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.DataModels.Receipts;

namespace Maasgroep.Database.Receipts
{

    public class ReceiptRepository : EditableRepository<Receipt, ReceiptModel, ReceiptData, ReceiptHistory>, IReceiptRepository
    {

		protected readonly ReceiptStatusRepository Statuses;
		public ReceiptRepository(MaasgroepContext db) : base(db)
			=> Statuses = new ReceiptStatusRepository();

		/** Create ReceiptModel from Receipt record */
		public override ReceiptModel GetModel(Receipt receipt)
        {
			var status = Statuses.GetModel(receipt.ReceiptStatus);
			var costCentre = Db.CostCentre.FirstOrDefault(c => c.Id == receipt.CostCentreId);

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

		/** Create or update Receipt record from data model */
        public override Receipt? GetRecord(ReceiptData data, Receipt? existingReceipt = null)
        {
            var receipt = existingReceipt ?? new();
			if (receipt.ReceiptStatus == ReceiptStatus.Goedgekeurd.ToString()
				|| receipt.ReceiptStatus == ReceiptStatus.Uitbetaald.ToString())
				return null; // Al definitief, dus aanpassen niet meer toegestaan

			var receiptHeeftFotos = existingReceipt == null ? false : Db.ReceiptPhoto.Where(p => p.ReceiptId == existingReceipt.Id).Any();
			
			receipt.Note = data.Note;
			receipt.Amount = data.Amount;
			receipt.CostCentreId = data.CostCentreId;
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

		/** List receipts by cost centre */
		public IEnumerable<ReceiptModel> ListByCostCentre(long costCentreId, int offset = default, int limit = default, bool includeDeleted = default)
			=> GetList(item => item.CostCentreId == costCentreId, null, offset, limit, includeDeleted).Select(item => GetModel(item)!);
    }
}
