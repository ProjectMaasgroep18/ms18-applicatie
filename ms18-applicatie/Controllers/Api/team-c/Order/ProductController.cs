using Maasgroep.Database.Orders;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;

namespace Maasgroep.Controllers.Api;

public class ProductController : EditableRepositoryController<IProductRepository, Product, ProductModel, ProductData, ProductHistory>
{
    public ProductController(IProductRepository repository) : base(repository) {}
    
    protected override bool AllowCreate(ProductData product)
        => HasPermission("order.product");

    protected override bool AllowDelete(Product product) // +Edit
        => HasPermission("order.product") && product.MemberCreatedId == CurrentMember!.Id;

}