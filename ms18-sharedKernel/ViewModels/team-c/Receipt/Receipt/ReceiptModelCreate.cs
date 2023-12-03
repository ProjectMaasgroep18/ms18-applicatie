
namespace Maasgroep.SharedKernel.ViewModels.Receipts
{
	public class ReceiptModelCreate
	{
		public decimal? Amount { get; set; }
		public string? Note { get; set; }
		public List<PhotoModelCreate>? Photos { get; set; }
		public string? CostCentre { get; set; }
	}
}
