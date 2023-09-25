using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maasgroep.Database
{
	public record Receipt
	{
		[Key]
		public long Id { get; set; }
		public DateTime Created { get; set; }
		[Column(TypeName = "decimal(18,2)")]
		public decimal Amount { get; set; }
		public long Store { get; set; }
		public long CostCentre { get; set; }
		public short ReceiptStatus { get; set; }
		public string? Location { get; set; }//TODO: Kevin; GPS zie ik nog even niet vliegen?




		//Ef
		public Store? StoreInstance { get; set; }
		public CostCentre? CostCentreInstance { get; set; }
		public ReceiptStatus ReceiptStatusInstance { get; set; }
		public Photo? Photo { get; set; }
		public ReceiptApproval? ReceiptApproval { get; set; }
	}
}