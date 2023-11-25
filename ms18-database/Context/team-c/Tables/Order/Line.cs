using Maasgroep.Database.Members;

namespace Maasgroep.Database.Order
{
    public record Line : GenericRecordActive
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long Quantity { get; set; }
        public long? MemberId { get; set; }
        public bool IsGuest { get; set; }
        public string? Note { get; set; }


        // Ef
        public Product Product { get; set; }
        public Member? Member { get; set; }
    }
}
