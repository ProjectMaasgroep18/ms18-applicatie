using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.SharedKernel.Interfaces.Receipts
{
	public interface IReceiptRepository
	{
		#region Receipt
		long Add(ReceiptModelCreateDb receipt);
		bool Modify(ReceiptModelUpdateDb receipt);
		ReceiptModel GetReceipt(long id);
		IEnumerable<ReceiptModel> GetReceipts(int offset, int fetch);
		bool DeleteReceipt(ReceiptModel receipt);
		IEnumerable<ReceiptModel> GetReceiptsByMember(long memberId, int offset, int fetch);
		#endregion

		#region Photo
		long Add(PhotoModelCreateDb photo);
		PhotoModel GetPhoto(long id);
		IEnumerable<PhotoModel> GetPhotosByReceipt(long receiptId, int offset, int fetch);
		bool DeletePhoto(long id);
		#endregion

		#region Approval
		bool AddApproval(ReceiptApprovalModelCreateDb approval);
		#endregion

		#region CostCentre
		IEnumerable<CostCentreModel> GetCostCentres(int offset, int fetch);
		CostCentreModel GetCostCentre(long id);
		long Add(CostCentreModelCreateDb costCentre);
		bool Modify(CostCentreModelUpdateDb costCentre);
		bool DeleteCostCentre(long id);
		IEnumerable<CostCentreModel> GetReceiptsByCostCentre(long costCentreId, int offset, int fetch);
		#endregion
	}
}
