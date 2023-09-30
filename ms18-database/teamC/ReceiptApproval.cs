using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maasgroep.Database
{
	public record ReceiptApproval
	{
		[Key]
		public long ReceiptId { get; set; }

		[Column(TypeName = "varchar(2048)")]
		public string Note { get; set; }

        //Generic
        public long UserCreatedId { get; set; }
        public long? UserModifiedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }


		// Ef
		public Receipt Receipt { get; set; }
		public Member UserCreated { get; set; }
		public Member? UserModified { get; set; }
	}
}
