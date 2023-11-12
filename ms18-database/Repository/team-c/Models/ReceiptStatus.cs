using Maasgroep.Database.Interfaces;

namespace Maasgroep.Database.Models
{
    public class ReceiptStatus : IReceiptStatus
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
