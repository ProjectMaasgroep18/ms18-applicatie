using Maasgroep.Database.Orders;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;

namespace Maasgroep.Database.Interfaces
{
    /** Stock repository interface, connecting to Stock database records */
    public interface IStockRepository : IEditableRepository<Stock, StockModel, StockData, StockHistory>
    {

    }
}
