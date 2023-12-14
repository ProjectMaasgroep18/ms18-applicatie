using Maasgroep.Database.Receipts;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.Controllers.Api;

public class ReceiptPhotoController : DeletableRepositoryController<IReceiptPhotoRepository, ReceiptPhoto, ReceiptPhotoModel>
{
    public ReceiptPhotoController(IReceiptPhotoRepository repository) : base(repository) {}
}