using System.Text.Json.Serialization;
using Ms18.Database.Models.TeamC.Receipt;

namespace Ms18.Database.Repository.TeamC.ViewModel
{
    public class ReceiptStatusViewModel
    {

        public long? ID { get; set; }
        public string Name { get; set; }

        public ReceiptStatusViewModel(ReceiptStatus dbRec)
        {
            ID = dbRec.Id;
            Name = dbRec.Name;
        }

        [JsonConstructor]
        public ReceiptStatusViewModel() { }

    }
}
