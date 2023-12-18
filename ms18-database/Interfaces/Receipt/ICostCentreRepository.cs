using Maasgroep.Database.Receipts;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.DataModels.Receipts;

namespace Maasgroep.Database.Interfaces
{
	/** Cost Centre repository interface, connecting to Cost Centre database records */
	public interface ICostCentreRepository : IEditableRepository<CostCentre, CostCentreModel, CostCentreData, CostCentreHistory>
	{

	}
}
