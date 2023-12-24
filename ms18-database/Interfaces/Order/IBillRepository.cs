using Maasgroep.Database.Orders;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;

namespace Maasgroep.Database.Interfaces
{
    /** Bill repository interface, connecting to Bill database records */
    public interface IBillRepository : IDeletableRepository<Bill, BillModel, BillData>
    {
        BillTotalModel GetTotal(long? MemberId = null);
    }
}
