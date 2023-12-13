
namespace Maasgroep.SharedKernel.ViewModels.Receipts
{
	public class PhotoModel
	{
		public PhotoModel() { }

		public long Id { get; set; }
		public string Base64Image { get; set; }
		public string FileExtension { get; set; }
		public string FileName { get; set; }
		public long ReceiptId { get; set; }
	}
}
