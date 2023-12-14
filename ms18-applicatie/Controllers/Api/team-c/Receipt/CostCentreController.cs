using Maasgroep.Database.Receipts;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.DataModels.Receipts;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public class CostCentreController : EditableRepositoryController<ICostCentreRepository, CostCentre, CostCentreModel, CostCentreData, CostCentreHistory>
{
	protected readonly IReceiptRepository Receipts;

    public CostCentreController(ICostCentreRepository repository, IReceiptRepository receipts) : base(repository)
        => Receipts = receipts;

    [HttpGet("{id}/Receipt")]
    public IActionResult CostCentreGetReceipts(long id, [FromQuery] int offset = default, [FromQuery] int limit = default, [FromQuery] bool includeDeleted = default)
        => Ok(Receipts.ListByCostCentre(id, offset, limit, includeDeleted));
}