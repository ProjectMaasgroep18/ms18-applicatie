using Maasgroep.Database.Admin;

namespace Maasgroep.Database.Orders
{
    public record Bill : GenericRecordActive
    {
        public bool IsGuest { get; set; }
        public string? Note { get; set; }
        public string? Name { get; set; }
        public double TotalAmount { get; set; }
        
        // Ef
        public ICollection<Line> Lines { get; set; }
    }
}
