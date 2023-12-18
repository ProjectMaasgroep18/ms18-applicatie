using Maasgroep.Database.Orders;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;

namespace Maasgroep.Database.Interfaces
{
    /** Product repository interface, connecting to Product database records */
    public interface IProductRepository : IEditableRepository<Product, ProductModel, ProductData, ProductHistory>
    {

    }
}
