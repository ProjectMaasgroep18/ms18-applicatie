using Ms18.Database.Models.TeamC.Admin;
using Ms18.Database.Repository.TeamC.ViewModel;

namespace Ms18.Database.Models.TeamC.Receipt
{
    public record Receipt
    {
        public long Id { get; set; }
        public decimal? Amount { get; set; }
        public long? StoreId { get; set; }
        public long? CostCentreId { get; set; }
        public long ReceiptStatusId { get; set; }
        public string? Location { get; set; }
        public string? Note { get; set; }


        // Generic
        public long MemberCreatedId { get; set; }
        public long? MemberModifiedId { get; set; }
        public long? MemberDeletedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public DateTime? DateTimeDeleted { get; set; }


        // EF receipt properties
        public CostCentre? CostCentre { get; set; }
        public ReceiptStatus ReceiptStatus { get; set; }
        public ICollection<ReceiptPhoto>? Photos { get; set; }
        public ReceiptApproval? ReceiptApproval { get; set; }


        // EF generic properties
        public Member MemberCreated { get; set; }
        public Member? MemberModified { get; set; }
        public Member? MemberDeleted { get; set; }

        public static Receipt FromViewModel(ReceiptViewModel viewModel)
        {
            return new Receipt
            {
                Amount = viewModel.Amount,
                Note = viewModel.Note,
            };
        }

    }
}