
using Maasgroep.Database.Order;

namespace Maasgroep.Database.Repository.ViewModel
{
    public class StockViewModel
    {
        public StockViewModel() { }

        public StockViewModel(string name, long quantity)
        {
            Name = name;
            Quantity = quantity;
        }

		public StockViewModel(Stock stock)
		{
            Name = "TODO";
			Quantity = stock.Quantity;
		}

		public long Quantity { get; set; }
        public string Name { get; set; }
    }
}
