using System.ComponentModel.DataAnnotations;

namespace Maasgroep.Database
{
	public record CostCentre
	{
		[Key]
		public long Id { get; set; }
		public string Name { get; set; } //Unique constraint in Builder.


		//Ef
		public ICollection<Receipt> Receipt { get; set; }
	}
}
