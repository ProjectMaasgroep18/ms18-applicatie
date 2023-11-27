using System.Text.Json.Serialization;
using Ms18.Database.Models.TeamC.Stock;

namespace Ms18.Database.Repository.TeamC.ViewModel;

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