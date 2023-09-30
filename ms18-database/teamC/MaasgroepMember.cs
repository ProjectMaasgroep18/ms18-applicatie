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
		public ICollection<CostCentre> CostCentresCreated { get; set; }
        public ICollection<CostCentre> CostCentresModified { get; set; }
        public ICollection<Store> StoresCreated { get; set; }
        public ICollection<Store> StoresModified { get; set; }
        public ICollection<ReceiptStatus> ReceiptStatusesCreated { get; set; }
        public ICollection<ReceiptStatus> ReceiptStatusesModified { get; set; }
        public ICollection<Receipt> ReceiptsCreated { get; set; }
        public ICollection<Receipt> ReceiptsModified { get; set; }
        public ICollection<ReceiptApproval> ReceiptApprovalsCreated { get; set; }
        public ICollection<ReceiptApproval> ReceiptApprovalsModified { get; set; }

    }
}
