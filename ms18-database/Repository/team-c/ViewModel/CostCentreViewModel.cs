using System.Text.Json.Serialization;
using Maasgroep.Database.Receipts;

namespace Maasgroep.Database.Repository.ViewModel
{
    public class CostCentreViewModel
    {
        
        public long? ID { get; set; }
        public string Name { get; set; }

        public CostCentreViewModel(CostCentre dbRec)
        {
            ID = dbRec.Id;
            Name = dbRec.Name;
        }
        
        [JsonConstructor]
        public CostCentreViewModel() { }
        
    }
}
