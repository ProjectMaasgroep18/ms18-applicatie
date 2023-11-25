using Maasgroep.Database.Context;

namespace Maasgroep.Database.Receipts
{
    public record ReceiptStatusHistory : GenericRecordHistory
    {
        public long Id { get; set; }
        public DateTime RecordCreated { get; set; }
        public long ReceiptStatusId { get; set; }
        public string Name { get; set; }
    }
}
