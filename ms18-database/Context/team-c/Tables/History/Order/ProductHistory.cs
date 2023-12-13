
namespace Maasgroep.Database.Orders
{
    public record ProductHistory : GenericRecordHistory
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
    }
}
