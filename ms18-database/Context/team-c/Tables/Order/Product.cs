using Maasgroep.Database.Context;

namespace Maasgroep.Database.Order
{
    public record Product : GenericRecord
    {
        public long Id { get; set; }
        public string Name { get; set; }

        // EF
        public Stock Stock { get; set; }
        public ProductPrice ProductPrice { get; set; }
        public ICollection<Line> OrderLines { get; set; }
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