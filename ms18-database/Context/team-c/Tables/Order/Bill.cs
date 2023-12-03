using Maasgroep.Database.Members;

namespace Maasgroep.Database.Order
{
    public record Bill : GenericRecordActive
	{
        public long Id { get; set; }
        public long? MemberId { get; set; }
        public bool IsGuest { get; set; }
        public string? Note { get; set; }
        public string? Name { get; set; }

        // Ef
        public Member? Member { get; set; }
        public ICollection<Line> Lines { get; set; }
    }
}
