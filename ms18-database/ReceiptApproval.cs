using System.ComponentModel.DataAnnotations;

namespace Maasgroep.Database
{
	public record ReceiptApproval
	{
		[Key]
		public long ReceiptId { get; set; }
		public DateTime Approved { get; set; }
		public int ApprovedBy { get; set; }
		public string Note { get; set; }


		// Ef
		public Receipt Receipt { get; set; }
		public MaasgroepMember MaasGroepMember { get; set; }
	}
}
