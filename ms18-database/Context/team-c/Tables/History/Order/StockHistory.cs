
namespace Maasgroep.Database.Order
{
    public record StockHistory : GenericRecordHistory
    {
        public StockHistory() { }

        public StockHistory(Stock current)
        {
            ProductId = current.ProductId;
            Quantity = current.Quantity;

            MemberCreatedId = current.MemberCreatedId;
            MemberModifiedId = current.MemberModifiedId;
            MemberDeletedId = current.MemberDeletedId;

            DateTimeCreated = current.DateTimeCreated;
            DateTimeModified = current.DateTimeModified;
            DateTimeDeleted = current.DateTimeDeleted;
        }

        public long Id { get; set; }
        public DateTime RecordCreated { get; set; }
        public long ProductId { get; set; }
        public long Quantity { get; set; }
    }
}
