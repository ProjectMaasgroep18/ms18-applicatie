using System.Text.Json.Serialization;
using Maasgroep.Database.Members;

namespace Maasgroep.Database.Receipts
{
    public record CostCentreViewModel
	{
		
		public long? Id { get; set; }
		public string? Name { get; set; }
        public DateTime DateTimeCreated { get; set; }
		public DateTime? DateTimeModified { get; set; }
        
		[JsonConstructor]
		public CostCentreViewModel() { }
		
		public CostCentreViewModel(CostCentre costCentre)
		{
			Id = costCentre.Id;
			Name = costCentre.Name;
			DateTimeCreated = costCentre.DateTimeCreated;
			DateTimeModified = costCentre.DateTimeModified;
		}
		
	}
}
