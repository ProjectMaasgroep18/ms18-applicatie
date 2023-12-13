using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.SharedKernel.Interfaces.Receipts
{
	public interface IReceiptPhotoRepository<TReceiptPhotoRecord, TReceiptPhotoHistory> : IGenericRepository<TReceiptPhotoRecord, PhotoModel, TReceiptPhotoHistory>
	{
		
	}
}
