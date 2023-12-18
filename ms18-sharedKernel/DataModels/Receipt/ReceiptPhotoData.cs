
namespace Maasgroep.SharedKernel.DataModels.Receipts
{
	public record ReceiptPhotoData
	{
		public string Base64Image { get; set; }
		public string FileExtension { get; set; }
		public string FileName { get; set; }
		public long ReceiptId { get; set; }
	}
}
