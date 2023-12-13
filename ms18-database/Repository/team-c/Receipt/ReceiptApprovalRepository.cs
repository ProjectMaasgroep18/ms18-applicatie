using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.Database.Receipts
{

    public class ReceiptApprovalRepository : WritableRepository<ReceiptApproval, ReceiptApprovalModel>, IReceiptApprovalRepository<ReceiptApproval>
    {
		public ReceiptApprovalRepository(MaasgroepContext db) : base(db) {}

        /** Create ReceiptApprovalModel from ReceiptApproval record */
        public override ReceiptApprovalModel GetModel(ReceiptApproval approval)
        {
            return new ReceiptApprovalModel() {
				Approved = approval.Approved,
				Note = approval.Note,
				ReceiptId = approval.ReceiptId,
			};
        }

        /** Create or update ReceiptApproval record from model */
        public override ReceiptApproval? GetRecord(ReceiptApprovalModel model, ReceiptApproval? existingApproval = null)
        {
            if (existingApproval != null)
                return null; // Approval records are not editable

            var approval = new ReceiptApproval() {
                Approved = model.Approved,
				Note = model.Note,
				ReceiptId = model.ReceiptId,
            };
			return approval;
        }
    }
}
