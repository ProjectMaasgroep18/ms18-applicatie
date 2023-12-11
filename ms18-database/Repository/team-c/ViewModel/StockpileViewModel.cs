using System.Text.Json.Serialization;
using Maasgroep.Database.Stock;

namespace Maasgroep.Database.Repository.ViewModel;

public class StockpileViewModel
{

    public long? ProductId { get; set; }
    
    public long Quantity { get; set; }

    
    [JsonConstructor]
    public StockpileViewModel(){}
    
    public StockpileViewModel(Stockpile dbRec)
    {
        ProductId = dbRec.ProductId;
        Quantity = dbRec.Quantity;
    }

}