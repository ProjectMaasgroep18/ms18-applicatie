using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Maasgroep.Database.Receipts;

namespace Maasgroep.Database.Repository.ViewModel
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
