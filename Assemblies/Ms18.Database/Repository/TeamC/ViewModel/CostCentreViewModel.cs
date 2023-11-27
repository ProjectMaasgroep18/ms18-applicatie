using System.Text.Json.Serialization;
using Ms18.Database.Models.TeamC.Receipt;

namespace Ms18.Database.Repository.TeamC.ViewModel
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
