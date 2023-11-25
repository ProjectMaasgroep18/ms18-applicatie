using Maasgroep.Database.Context;

namespace Maasgroep.Database.Order
{
    public record ProductPrice : GenericRecord
    {
        public long ProductId { get; set; }
        public decimal Price { get; set; }

        // Ef
        public Product Product { get; set; }
    }
}
