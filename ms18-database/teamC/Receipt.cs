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
		public decimal? Amount { get; set; }
		public long? Store { get; set; }
		public long? CostCentre { get; set; }
		public long ReceiptStatus { get; set; }
		public string? Location { get; set; }//TODO: Kevin; GPS zie ik nog even niet vliegen?
		[Column(TypeName = "varchar(2048)")]
		public string? Note { get; set; }


		//Generic
		public long UserCreated { get; set; }
		public long? UserModified { get; set; }
		public DateTime? DateTimeModified { get; set; }



		//Ef
		public Store? StoreInstance { get; set; }
		public CostCentre? CostCentreInstance { get; set; }
		public ReceiptStatus ReceiptStatusInstance { get; set; }
		public Photo? Photo { get; set; }
		public ReceiptApproval? ReceiptApproval { get; set; }
		public MaasgroepMember UserCreatedInstance { get; set; }
		public MaasgroepMember? UserModifiedInstance { get; set; }
	}
}