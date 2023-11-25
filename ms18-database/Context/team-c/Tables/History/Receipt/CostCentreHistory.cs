using Maasgroep.Database.Context;

namespace Maasgroep.Database.Receipts
{
    public record CostCentreHistory : GenericRecordHistory
    {
        public long Id { get; set; }
        public DateTime RecordCreated { get; set; }
        public long CostCentreId { get; set; }
        public string Name { get; set; }
    }
}
