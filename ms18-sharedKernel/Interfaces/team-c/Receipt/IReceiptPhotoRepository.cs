using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.SharedKernel.Interfaces.Receipts
{
	public interface IReceiptPhotoRepository<TReceiptPhotoRecord> : IDeletableRepository<TReceiptPhotoRecord, PhotoModel>
	{
		
	}
}
