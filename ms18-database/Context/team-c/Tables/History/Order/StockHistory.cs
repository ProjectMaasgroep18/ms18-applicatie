
namespace Maasgroep.Database.Orders
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

        public long ProductId { get; set; }
        public long Quantity { get; set; }
    }
}
