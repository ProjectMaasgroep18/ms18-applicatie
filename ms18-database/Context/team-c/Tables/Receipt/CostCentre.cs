
namespace Maasgroep.Database.Receipts
{
    public record CostCentre : GenericRecordActive
	{
		public string Name { get; set; }


        // EF receipt properties
        public ICollection<Receipt> Receipt { get; set; }
    }
}
