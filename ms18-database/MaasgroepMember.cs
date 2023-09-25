using System.ComponentModel.DataAnnotations;

namespace Maasgroep.Database
{
	public record MaasgroepMember
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }


		//Ef
		public ReceiptApproval? ReceiptApproval { get; set; }
	}
}
