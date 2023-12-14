
namespace Maasgroep.Database.Orders
{
    public record Product : GenericRecordActive
	{
        public string Name { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public double Price { get; set; }

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