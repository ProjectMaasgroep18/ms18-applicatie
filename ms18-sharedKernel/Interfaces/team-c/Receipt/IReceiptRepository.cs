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
		bool Delete(ReceiptModelDeleteDb receipt);
		IEnumerable<ReceiptModel> GetReceiptsByMember(long memberId, int offset, int fetch);
		#endregion

		#region Photo
		long Add(PhotoModelCreateDb photo);
		PhotoModel GetPhoto(long id);
		IEnumerable<PhotoModel> GetPhotosByReceipt(long receiptId, int offset, int fetch);
		bool Delete(PhotoModelDeleteDb id);
		#endregion

		#region Approval
		bool AddApproval(ReceiptApprovalModelCreateDb approval);
		#endregion

		#region CostCentre
		IEnumerable<CostCentreModel> GetCostCentres(int offset, int fetch);
		CostCentreModel GetCostCentre(long id);
		long Add(CostCentreModelCreateDb costCentre);
		bool Modify(CostCentreModelUpdateDb costCentre);
		bool Delete(CostCentreModelDeleteDb id);
		IEnumerable<ReceiptModel> GetReceiptsByCostCentre(long costCentreId, int offset, int fetch);
		#endregion
	}
}
