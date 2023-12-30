using Maasgroep.Database.Receipts;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.DataModels.Receipts;
using Maasgroep.Interfaces;

namespace Maasgroep.Controllers.Api;

public class ReceiptPhotoController : DeletableRepositoryController<IReceiptPhotoRepository, ReceiptPhoto, ReceiptPhotoModel, ReceiptPhotoData>
{
    public override string ItemName { get => "Foto"; }
    public ReceiptPhotoController(IReceiptPhotoRepository repository, IMaasgroepAuthenticationService maasgroepAuthenticationService) : base(repository, maasgroepAuthenticationService) {}
    
    protected override bool AllowList()
        => HasPermission("receipt.approve") || HasPermission("receipt.pay");

    protected override bool AllowView(ReceiptPhoto? receiptPhoto)
        => HasPermission("receipt.approve") || HasPermission("receipt.pay") || (CurrentMember != null && receiptPhoto?.MemberCreatedId == CurrentMember.Id);

    protected override bool AllowCreate(ReceiptPhotoData receiptPhoto)
        => HasPermission("receipt");

    protected override bool AllowDelete(ReceiptPhoto? receipt) // +Edit
        => HasPermission("admin") || receipt?.MemberCreatedId == CurrentMember?.Id;
}