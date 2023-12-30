using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.SharedKernel.ViewModels.Orders
{
    public record BillModel
    {
        public long Id { get; set; }
        public List<LineModel> Lines { get; set; }
        public MemberModel? MemberCreated { get; set; }
        public bool IsGuest { get; set; }
        public string? Note { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public double TotalAmount { get; set; }
        public DateTime? DateTimeCreated { get; set; }
    }
}
