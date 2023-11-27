using Maasgroep.Database.Members;
using Maasgroep.Database.Repository.ViewModel;

namespace Maasgroep.Database.Receipts
{
    public record ReceiptApproval : GenericRecordActive
	{
		public long ReceiptId { get; set; }
		public string? Note { get; set; }

        // EF receipt properties
        public Receipt Receipt { get; set; }


        // EF generic properties
        public Member MemberCreated { get; set; }
        public Member? MemberModified { get; set; }
        public Member? MemberDeleted { get; set; }

        public static ReceiptApproval FromViewModel(ReceiptApprovalViewModel viewModel)
        {
	        return new ReceiptApproval
	        {
		        ReceiptId = viewModel.ReceiptId ?? 0,
		        Note = viewModel.Note,
	        };
        }
    }
}
