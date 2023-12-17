using Maasgroep.Database.Receipts;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.DataModels.Receipts;

namespace Maasgroep.Controllers.Api;

public class ReceiptPhotoController : DeletableRepositoryController<IReceiptPhotoRepository, ReceiptPhoto, ReceiptPhotoModel, ReceiptPhotoData>
{
    public override string ItemName { get => "Foto"; }
    public ReceiptPhotoController(IReceiptPhotoRepository repository) : base(repository) {}
}