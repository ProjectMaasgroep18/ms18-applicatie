using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.SharedKernel.ViewModels.Orders
{
    public record BillTotalModel
    {
        public long BillCount { get; set; }
        public long ProductQuantity { get; set; }
        public double TotalAmount { get; set; }
    }
}
