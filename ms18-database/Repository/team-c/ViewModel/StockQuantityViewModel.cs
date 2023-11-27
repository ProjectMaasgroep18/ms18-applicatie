using System.Text.Json.Serialization;

namespace Maasgroep.Database.Repository.ViewModel;

public class StockQuantityViewModel
{
    
    public long Quantity { get; set; }
    
    [JsonConstructor]
    public StockQuantityViewModel(){}
    
}