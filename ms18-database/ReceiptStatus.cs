using System.ComponentModel.DataAnnotations;

namespace Maasgroep.Database
{
	public record ReceiptStatus
	{
		[Key]
		public short Id { get; set; }
		public string Name { get; set; }


		//Ef
		public ICollection<Receipt> Receipt { get; set; }
	}
}
