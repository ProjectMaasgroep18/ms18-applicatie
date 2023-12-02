using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.SharedKernel.Interfaces.Receipts
{
	public interface IReceiptModelConvertService
	{
		ReceiptModel Convert(ReceiptModelCreate receiptModelCreate);
	}
}
