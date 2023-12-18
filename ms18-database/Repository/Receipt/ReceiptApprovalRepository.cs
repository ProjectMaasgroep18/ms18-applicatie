using Maasgroep.Database.Admin;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.DataModels.Receipts;

namespace Maasgroep.Database.Receipts
{
    public class ReceiptApprovalRepository : WritableRepository<ReceiptApproval, ReceiptApprovalModel, ReceiptApprovalData>, IReceiptApprovalRepository
    {
        protected ReceiptRepository Receipts;
        protected MemberRepository Members;
        public ReceiptApprovalRepository(MaasgroepContext db) : base(db) {
            Receipts = new(db);
            Members = new(db);
        }

        /** Create ReceiptApprovalModel from ReceiptApproval record */
        public override ReceiptApprovalModel GetModel(ReceiptApproval approval)
        {
            var member = approval.MemberCreatedId != null ? Members.GetModel((long)approval.MemberCreatedId) : null;

            return new ReceiptApprovalModel() {
                Id = approval.Id,
                Approved = approval.Approved,
                Note = approval.Note,
                ReceiptId = approval.ReceiptId,
                MemberCreated = member,
                DateTimeCreated = approval.DateTimeCreated,
            };
        }

        /** Create or update ReceiptApproval record from data model */
        public override ReceiptApproval? GetRecord(ReceiptApprovalData data, ReceiptApproval? existingApproval = null)
        {
            if (existingApproval != null)
                return null; // Approval records are not editable

            var receipt = Receipts.GetModel(data.ReceiptId);
            if (receipt == null)
                return null; // Receipt not found

            if (data.Paid) {
                if (!receipt.IsPayable)
                    return null; // Not allowed to mark paid
            } else {
                if (!receipt.IsApprovable)
                    return null; // Not allowed to approve
            }

            var approval = new ReceiptApproval() {
                Approved = data.Approved,
                Paid = data.Paid,
                Note = data.Note,
                ReceiptId = data.ReceiptId,
            };
            return approval;
        }

        /** List approvals by receipt */
        public IEnumerable<ReceiptApprovalModel> ListByReceipt(long receiptId, int offset = default, int limit = default)
            => GetList(item => item.ReceiptId == receiptId, null, offset, limit).Select(item => GetModel(item)!);

        /** Save updated receipt status when saving approval */
        public override Action<MaasgroepContext> GetSaveAction(ReceiptApproval record)
        {
            var saveAction = base.GetSaveAction(record);
            return (MaasgroepContext db) => {
                saveAction.Invoke(db);
                var receipt = Receipts.GetById(record.ReceiptId);
                if (receipt == null)
                    return;
                receipt.ReceiptStatus = (record.Paid ? ReceiptStatus.Uitbetaald : record.Approved ? ReceiptStatus.Goedgekeurd : ReceiptStatus.Afgekeurd).ToString();
                db.Receipt.Update(receipt);
            };
        }
    }
}
