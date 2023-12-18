namespace Maasgroep.SharedKernel.DataModels.Orders
{
    public record LineData
    {
        public long ProductId { get; set; }
        public long Quantity { get; set; }
    }
}
