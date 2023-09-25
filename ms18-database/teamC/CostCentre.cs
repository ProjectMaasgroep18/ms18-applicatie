using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maasgroep.Database
{
	public record CostCentre
	{
		[Key]
		public long Id { get; set; }
		[Column(TypeName = "varchar(256)")]
		public string Name { get; set; } //Unique constraint in Builder.

		//Generic
		public int UserCreated { get; set; }
		public int UserModified { get; set; }
		public DateTime DateTimeCreated { get; set; }
		public DateTime DateTimeModified { get; set; }


		//Ef
		public ICollection<Receipt> Receipt { get; set; }
		public MaasgroepMember UserCreatedInstance { get; set; }
		public MaasgroepMember UserModifiedInstance { get; set; }
	}
}
