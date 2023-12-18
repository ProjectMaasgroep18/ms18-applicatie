
namespace Maasgroep.SharedKernel.ViewModels.Receipts
{
	public class ReceiptApprovalModelCreate
	{
		public long ReceiptId { get; set; }
		public string Note { get; set; }
		public bool Approved { get; set; }
	}
}
