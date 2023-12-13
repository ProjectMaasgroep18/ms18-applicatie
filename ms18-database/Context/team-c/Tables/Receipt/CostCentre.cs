
namespace Maasgroep.Database.Receipts
{
    public record CostCentre : GenericRecordActive
	{
		public long Id { get; set; }
		public string Name { get; set; }


        // EF receipt properties
        public ICollection<Receipt> Receipt { get; set; }
    }
}
