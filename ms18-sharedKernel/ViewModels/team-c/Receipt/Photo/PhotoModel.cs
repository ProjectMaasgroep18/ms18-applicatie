
namespace Maasgroep.SharedKernel.ViewModels.Receipts
{
	public class PhotoModel
	{
		public PhotoModel() { }

		public long Id { get; set; }
		public string Base64Image { get; set; }
		public string fileExtension { get; set; }
		public string fileName { get; set; }
	}
}
