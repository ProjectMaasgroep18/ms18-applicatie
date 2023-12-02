using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.SharedKernel.Interfaces.Receipts
{
	public interface IReceiptRepository
	{
		long AddReceipt(ReceiptModelCreateDb receipt);
		bool ModifyReceipt(ReceiptModelUpdateDb receipt);
		ReceiptModel GetReceipt(long id);
		IEnumerable<ReceiptModel> GetReceipts(int offset, int fetch);
		bool DeleteReceipt(ReceiptModel receipt);
	}
}
