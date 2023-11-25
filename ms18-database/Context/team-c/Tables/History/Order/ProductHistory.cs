
namespace Maasgroep.Database.Order
{
    public record ProductHistory : GenericRecordHistory
    {
        public long Id { get; set; }
        public DateTime RecordCreated { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
    }
}
