
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

        public long Quantity { get; set; }
        public string Name { get; set; }
    }
}
