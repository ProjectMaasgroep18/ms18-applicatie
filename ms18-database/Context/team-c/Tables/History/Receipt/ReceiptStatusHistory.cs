
namespace Maasgroep.Database.Receipts
{
    public record ReceiptStatusHistory : GenericRecordHistory
    {
        public long ReceiptStatusId { get; set; }
        public string Name { get; set; }
    }
}
