namespace Maasgroep.SharedKernel.ViewModels.Receipts
{
	public class ReceiptModel
	{
		public ReceiptModel() 
		{
			CostCentre = new CostCentreModel();
			Photos = new List<ReceiptPhotoModel>();
			StatusString = ReceiptStatus.Onbekend.ToString();
		}
		public long Id { get; set; }
		public decimal? Amount { get; set; }
		public string? Location { get; set; }
		public string? Note { get; set; }
		public ReceiptStatus Status { get; set; }
		public string StatusString { get; set; }
		public CostCentreModel? CostCentre { get; set; }
		public List<ReceiptPhotoModel>? Photos { get; set; }
		public ReceiptApprovalModel Approval { get; set; }
		public long? MemberCreatedId { get; set; }
	}
}
