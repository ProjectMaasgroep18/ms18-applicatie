using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.SharedKernel.Interfaces.Receipts
{
	public interface ICostCentreRepository<TCostCentreRecord, TCostCentreHistory> : IGenericRepository<TCostCentreRecord, CostCentreModel, TCostCentreHistory>
	{
		
	}
}
