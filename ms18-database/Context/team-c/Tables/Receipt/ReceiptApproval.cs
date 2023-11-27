using Maasgroep.Database.Members;
using Maasgroep.Database.Repository.ViewModel;

namespace Maasgroep.Database.Receipts
{
    public record ReceiptApproval
	{
		public long ReceiptId { get; set; }
		public string? Note { get; set; }


        // Generic
        public long MemberCreatedId { get; set; }
        public long? MemberModifiedId { get; set; }
        public long? MemberDeletedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public DateTime? DateTimeDeleted { get; set; }


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
