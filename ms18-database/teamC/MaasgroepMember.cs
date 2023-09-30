using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maasgroep.Database
{
	public record MaasgroepMember
	{
		[Key]
		public long Id { get; set; }
		[Column(TypeName = "varchar(256)")]
		public string Name { get; set; }


        //Generic
        public long? UserCreated { get; set; }
        public long? UserModified { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }


        //Ef receipt properties
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

        //Ef generic properties
        public MaasgroepMember? UserCreatedInstance { get; set; }
        public MaasgroepMember? UserModifiedInstance { get; set; }
        public ICollection<MaasgroepMember>? MembersCreated { get; set; }
        public ICollection<MaasgroepMember>? MembersModified { get; set; }

    }
}
