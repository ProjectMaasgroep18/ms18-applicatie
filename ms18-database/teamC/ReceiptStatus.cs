
namespace Maasgroep.Database
{
	public record ReceiptStatus
	{
		public long Id { get; set; }
		public string Name { get; set; }


        // Generic
        public long UserCreatedId { get; set; }
		public long? UserModifiedId { get; set; }
		public DateTime DateTimeCreated { get; set; }
		public DateTime? DateTimeModified { get; set; }


		// EF receipt properties
		public ICollection<Receipt> Receipt { get; set; }


		// EF generic properties
		public Member UserCreated { get; set; }
		public Member? UserModified { get; set; }
	}
}
