using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.SharedKernel.ViewModels.Receipts
{
    public record ReceiptModel
    {
        public long Id { get; set; }
        public decimal? Amount { get; set; }
        public string? Location { get; set; }
        public string? Note { get; set; }
        public ReceiptStatus Status { get; set; }
        public string StatusString { get; set; }
        public CostCentreModel? CostCentre { get; set; }
        public MemberModel? MemberCreated { get; set; }
        public bool IsEditable { get; set; }
        public bool IsApprovable { get; set; }
        public bool IsPayable { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }
    }
}
