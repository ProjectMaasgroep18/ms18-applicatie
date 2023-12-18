
namespace Maasgroep.Database.Receipts
{
    public record CostCentreHistory : GenericRecordHistory
    {
        public long CostCentreId { get; set; }
        public string Name { get; set; }
    }
}
