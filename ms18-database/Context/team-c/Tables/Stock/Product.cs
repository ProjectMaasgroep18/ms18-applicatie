using Maasgroep.Database.Members;

namespace Maasgroep.Database.Stock
{
    public record Product
    {
        public long Id { get; set; }
        public string Name { get; set; }

        // Generic
        public long MemberCreatedId { get; set; }
        public long? MemberModifiedId { get; set; }
        public long? MemberDeletedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public DateTime? DateTimeDeleted { get; set; }

        // EF
        public Stockpile Stock { get; set; }

        // EF generic properties
        public Member MemberCreated { get; set; }
        public Member? MemberModified { get; set; }
        public Member? MemberDeleted { get; set; }

    }
}


/*
Voorbeeld Flutter
  StockProduct(
      product: Product(
        priceQuantity: 1,
        color: Colors.blueAccent,
        name: 'Cola',
        price: 3.13,
      ),
      quantity: 21,
*/ 