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
		public long StoreId { get; set; }
		public long CostCentreId { get; set; }
		public short ReceiptStatusId { get; set; }
		public string? Location { get; set; }//TODO: Kevin; GPS zie ik nog even niet vliegen?




		//Ef
		public Store? Store { get; set; }
		public CostCentre? CostCentre { get; set; }
		public ReceiptStatus ReceiptStatus { get; set; }
		public Photo? Photo { get; set; }
		public ReceiptApproval? ReceiptApproval { get; set; }
	}
}