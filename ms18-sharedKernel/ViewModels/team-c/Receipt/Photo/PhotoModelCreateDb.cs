
using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.SharedKernel.ViewModels.Receipts
{
	public class PhotoModelCreateDb
	{
		public PhotoModelCreate PhotoModel { get; set; }
		public MemberModel Member { get; set; }
		public long ReceiptId { get; set; }
	}
}
