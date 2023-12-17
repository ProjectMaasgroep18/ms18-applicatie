using Maasgroep.Database.Orders;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;

namespace Maasgroep.Controllers.Api;

public class ProductController : EditableRepositoryController<IProductRepository, Product, ProductModel, ProductData, ProductHistory>
{
    public ProductController(IProductRepository repository) : base(repository) {}
}