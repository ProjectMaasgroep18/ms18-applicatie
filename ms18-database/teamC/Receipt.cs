using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maasgroep.Database
{
	public record Receipt
	{
		[Key]
		public long Id { get; set; }
		[Column(TypeName = "decimal(18,2)")]
		public decimal? Amount { get; set; }
		public long? StoreId { get; set; }
		public long? CostCentreId { get; set; }
		public long ReceiptStatusId { get; set; }
		public string? Location { get; set; }//TODO: Kevin; GPS zie ik nog even niet vliegen?
		[Column(TypeName = "varchar(2048)")]
		public string? Note { get; set; }


		//Generic
		public long UserCreatedId { get; set; }
		public long? UserModifiedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }



		//Ef
		public Store? Store { get; set; }
		public CostCentre? CostCentre { get; set; }
		public ReceiptStatus ReceiptStatus { get; set; }
		public Photo? Photo { get; set; }
		public ReceiptApproval? ReceiptApproval { get; set; }
		public MaasgroepMember UserCreated { get; set; }
		public MaasgroepMember? UserModified { get; set; }
	}
}