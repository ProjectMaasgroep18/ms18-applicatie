using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.SharedKernel.Interfaces.Receipts
{
	public interface ICostCentreRepository<TRecord, THistory> : IEditableRepository<TRecord, CostCentreModel, THistory>
	{
		
	}
}
