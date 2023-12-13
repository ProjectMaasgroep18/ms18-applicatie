using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.Services;
using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Database.Receipts
{

	//TODO: deze gaat ook teveel doen (receipt + photo + costcentre).
    public class OLDReceiptRepository : IReceiptRepository
    {
        private readonly MaasgroepContext _db;
        public OLDReceiptRepository(MaasgroepContext db) 
        {
			_db = db;
        
		#region Approval
		public bool AddApproval(ReceiptApprovalModelCreateDb approval)
		{
			var approvalToAdd = new ReceiptApproval()
			{ 
				Approved = approval.Approval.Approved,
				Note = approval.Approval.Note,
				ReceiptId = approval.Approval.ReceiptId,
				MemberCreatedId = approval.Member.Id
			};

			_db.ReceiptApproval.Add(approvalToAdd);
			_db.SaveChanges();

			return true;
		}
		#endregion


		private ReceiptApprovalHistory CreateReceiptApprovalHistory(ReceiptApproval approval)
		{
			var history = new ReceiptApprovalHistory();

			history.ReceiptId = approval.ReceiptId;
			history.Approved = approval.Approved;
			history.Note = approval.Note;

			history.MemberCreatedId = approval.MemberCreatedId;
			history.MemberModifiedId = approval.MemberModifiedId;
			history.MemberDeletedId = approval.MemberDeletedId;
			history.DateTimeCreated = approval.DateTimeCreated;
			history.DateTimeModified = approval.DateTimeModified;
			history.DateTimeDeleted = approval.DateTimeDeleted;

			return history;
		}
	}
}
