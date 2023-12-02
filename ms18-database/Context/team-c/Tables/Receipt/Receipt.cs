using Maasgroep.Database.Repository.ViewModel;

namespace Maasgroep.Database.Receipts
{
    public record Receipt : GenericRecordActive
	{
		public long Id { get; set; }
		public decimal? Amount { get; set; }
		public long? CostCentreId { get; set; }
		public string ReceiptStatus { get; set; }
		public string? Location { get; set; }
		public string? Note { get; set; }


        // EF receipt properties
        public CostCentre? CostCentre { get; set; }
		public ICollection<Photo>? Photos { get; set; }
		public ReceiptApproval? ReceiptApproval { get; set; }
        
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