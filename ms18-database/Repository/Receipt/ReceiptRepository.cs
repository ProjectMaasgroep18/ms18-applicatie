using Maasgroep.Database.Admin;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.DataModels.Receipts;

namespace Maasgroep.Database.Receipts
{
    public class ReceiptRepository : EditableRepository<Receipt, ReceiptModel, ReceiptData, ReceiptHistory>, IReceiptRepository
    {
        protected readonly ReceiptStatusRepository Statuses;
        protected readonly CostCentreRepository CostCentres;
        protected readonly MemberRepository Members;
        public ReceiptRepository(MaasgroepContext db) : base(db) {
            Statuses = new ReceiptStatusRepository();
            CostCentres = new CostCentreRepository(db);
            Members = new MemberRepository(db);
        }

        /** Create ReceiptModel from Receipt record */
        public override ReceiptModel GetModel(Receipt receipt)
        {
            var status = Statuses.GetModel(receipt.ReceiptStatus);
            var costCentre = receipt.CostCentreId != null ? CostCentres.GetModel((long)receipt.CostCentreId) : null;
            var member = receipt.MemberCreatedId != null ? Members.GetModel((long)receipt.MemberCreatedId) : null;

            return new ReceiptModel() {
                Id = receipt.Id,
                Note = receipt.Note,
                Amount = receipt.Amount,
                Status = status,
                StatusString = status.ToString(),
                CostCentre = costCentre,
                IsEditable = status != ReceiptStatus.Goedgekeurd && status != ReceiptStatus.Uitbetaald,
                IsApprovable = status == ReceiptStatus.Ingediend,
                IsPayable = status == ReceiptStatus.Goedgekeurd,
                MemberCreated = member,
                DateTimeCreated = receipt.DateTimeCreated,
                DateTimeModified = receipt.DateTimeModified,
            };
        }

        /** Create or update Receipt record from data model */
        public override Receipt? GetRecord(ReceiptData data, Receipt? existingReceipt = null)
        {
            var receipt = existingReceipt ?? new();
            var model = GetModel(receipt);
            if (!model.IsEditable)
                return null; // Already approved or even paid

            var receiptHeeftFotos = existingReceipt != null && Db.ReceiptPhoto.Where(p => p.ReceiptId == existingReceipt.Id && p.DateTimeDeleted == null).Any();
            
            receipt.Note = data.Note;
            receipt.Amount = data.Amount == 0 ? null : data.Amount;
            receipt.CostCentreId = data.CostCentreId == 0 ? null :data.CostCentreId;
            if (receiptHeeftFotos
                && receipt.Note != null
                && receipt.Note?.Trim() != ""
                && receipt.Amount != null
                && receipt.Amount != 0
                && receipt.CostCentreId != null) {
                // All the necessary data is there
                receipt.ReceiptStatus = ReceiptStatus.Ingediend.ToString();
            } else {
                // We lack some of the necessary data
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

        /** List all receipts (with includeDeleted off, see below) */
        public override IEnumerable<ReceiptModel> ListAll(int offset = default, int limit = default)
            => ListAll(offset, limit, false);

        /** List all receipts (without Concepts, with Included first) */
        public override IEnumerable<ReceiptModel> ListAll(int offset = default, int limit = default, bool includeDeleted = default)
            => GetList(item => item.ReceiptStatus != ReceiptStatus.Concept.ToString(), item => Statuses.GetModel(item.ReceiptStatus) switch {
                // Sorteer op wat nog goedgekeurd moet worden
                ReceiptStatus.Ingediend => 1,
                _ => 0,
            }, offset, limit, includeDeleted).Select(item => GetModel(item)!);
        
        /** List receipts by member */
        public override IEnumerable<ReceiptModel> ListByMember(long memberId, int offset = default, int limit = default, bool includeDeleted = default)
            => GetList(item => item.MemberCreatedId == memberId, item => Statuses.GetModel(item.ReceiptStatus) switch {
                // Sorteer op wat nog afgemaakt/verbeterd moet worden
                ReceiptStatus.Afgekeurd => 2,
                ReceiptStatus.Concept => 1,
                _ => 0,
            }, offset, limit, includeDeleted).Select(item => GetModel(item)!);

        /** List receipts by cost centre */
        public IEnumerable<ReceiptModel> ListByCostCentre(long costCentreId, int offset = default, int limit = default, bool includeDeleted = default)
            => GetList(item => item.CostCentreId == costCentreId && item.ReceiptStatus != ReceiptStatus.Concept.ToString(), item => Statuses.GetModel(item.ReceiptStatus) switch {
                // Sorteer op wat nog goedgekeurd moet worden
                ReceiptStatus.Ingediend => 1,
                _ => 0,
            }, offset, limit, includeDeleted).Select(item => GetModel(item)!);

        /** List receipts waiting te be paid out */
        public IEnumerable<ReceiptModel> ListPayable(int offset = default, int limit = default, bool includeDeleted = default)
            => GetList(item => item.ReceiptStatus == ReceiptStatus.Goedgekeurd.ToString() || item.ReceiptStatus == ReceiptStatus.Uitbetaald.ToString(), item => Statuses.GetModel(item.ReceiptStatus) switch {
                // Sorteer op wat nog uitbetaald moet worden
                ReceiptStatus.Goedgekeurd => 1,
                _ => 0,
            }, offset, limit, includeDeleted).Select(item => GetModel(item)!);
    }
}
