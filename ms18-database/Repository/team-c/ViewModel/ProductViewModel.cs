using System.Text.Json.Serialization;
using Maasgroep.Database.Stock;

namespace Maasgroep.Database.Repository.ViewModel;

public class ProductViewModel
{

    public long? Id { get; set; }
    public string? Name { get; set; }
    
    public string? StockpileURI { get; set; }
    
    [JsonConstructor]
    public ProductViewModel(){}
    
    public ProductViewModel(Product dbRec)
    {
        Id = dbRec.Id;
        Name = dbRec.Name;
    }

}