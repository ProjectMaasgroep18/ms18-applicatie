
namespace Maasgroep.SharedKernel.ViewModels.Orders
{
    public record StockModel
    {    
        public ProductModel? Product { get; set; }
        public long Quantity { get; set; }
    }
}
