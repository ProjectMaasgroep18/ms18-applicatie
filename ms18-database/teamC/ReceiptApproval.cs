using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maasgroep.Database
{
	public record ReceiptApproval
	{
		[Key]
		public long Receipt { get; set; }

		[Column(TypeName = "varchar(2048)")]
		public string Note { get; set; }

        //Generic
        public long UserCreated { get; set; }
        public long? UserModified { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }


		// Ef
		public Receipt ReceiptInstance { get; set; }
		public MaasgroepMember UserCreatedInstance { get; set; }
		public MaasgroepMember? UserModifiedInstance { get; set; }
	}
}
