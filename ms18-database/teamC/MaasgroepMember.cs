using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maasgroep.Database
{
	public record MaasgroepMember
	{
		[Key]
		public int Id { get; set; }
		[Column(TypeName = "varchar(256)")]
		public string Name { get; set; }


		//Ef
		public ReceiptApproval? ReceiptApproval { get; set; }
		public ICollection<CostCentre> CostCentres { get; set; }
		public ICollection<Store> Stores { get; set; }
		public ICollection<ReceiptStatus> ReceiptStatuses { get; set; }
		public ICollection<Receipt> Receipts { get; set; }
		public ICollection<ReceiptApproval> ReceiptApprovals { get; set; }

	}
}
