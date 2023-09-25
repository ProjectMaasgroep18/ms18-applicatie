using System.ComponentModel.DataAnnotations;

namespace Maasgroep.Database
{
	public record ReceiptApproval
	{
		[Key]
		public long Receipt { get; set; }
		public DateTime Approved { get; set; }
		public int ApprovedBy { get; set; }
		public string Note { get; set; }


		// Ef
		public Receipt ReceiptInstance { get; set; }
		public MaasgroepMember MaasGroepMember { get; set; }
	}
}
