using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.SharedKernel.ViewModels.Receipts
{ 
	public class ReceiptApprovalModelCreateDb
	{
		public ReceiptApprovalModelCreate Approval { get; set; }
		public MemberModel Member { get; set; }
	}
}
