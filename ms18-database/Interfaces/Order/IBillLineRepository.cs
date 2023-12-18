using Maasgroep.Database.Orders;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;

namespace Maasgroep.Database.Interfaces
{
	/** Line repository interface, connecting to Line database records */
	public interface IBillLineRepository : IEditableRepository<Line, LineModel, LineData, LineHistory>
	{

	}
}
