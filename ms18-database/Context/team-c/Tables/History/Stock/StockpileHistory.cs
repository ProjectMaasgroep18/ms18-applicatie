﻿
namespace Maasgroep.Database.Stock
{
    public record StockpileHistory
    {
        public StockpileHistory() { }

        public StockpileHistory(Stockpile current)
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


        // Generic
        public long MemberCreatedId { get; set; }
        public long? MemberModifiedId { get; set; }
        public long? MemberDeletedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public DateTime? DateTimeDeleted { get; set; }

        //// EF
        //public Product Product { get; set; }

        //// EF generic properties
        //public Member MemberCreated { get; set; }
        //public Member? MemberModified { get; set; }
        //public Member? MemberDeleted { get; set; }
    }
}
