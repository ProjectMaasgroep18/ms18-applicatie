
namespace Maasgroep.SharedKernel.ViewModels.Receipts
{
	//KH: Dit hernoemd van 'Receipt' naar 'ReceiptModel'
	//    ivm gebruik van 'Receipt' in namespace '.Database'
	public class ReceiptModel
	{
		public ReceiptModel() 
		{
			CostCentre = new CostCentreModel();
			Photos = new List<PhotoModel>();
		}
		public long? Id { get; set; }
		public decimal? Amount { get; set; }
		public string? Location { get; set; }
		public string? Note { get; set; }
		public ReceiptStatus Status { get; set; }
		public string? StatusString { get; set; }
		public CostCentreModel CostCentre { get; set; }
		public List<PhotoModel> Photos { get; set; }
	}
}
