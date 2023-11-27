using System.Text.Json.Serialization;

namespace Ms18.Database.Repository.TeamC.ViewModel;

public class StockQuantityViewModel
{

    public long Quantity { get; set; }

    [JsonConstructor]
    public StockQuantityViewModel() { }

}