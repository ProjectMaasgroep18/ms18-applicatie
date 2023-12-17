using Maasgroep.Database.Orders;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public class ProductController : EditableRepositoryController<IProductRepository, Product, ProductModel, ProductData, ProductHistory>
{
    protected IStockRepository Stock;
    public ProductController(IProductRepository repository, IStockRepository stock) : base(repository)
        => Stock = stock;
    
    protected override bool AllowCreate(ProductData product)
        => HasPermission("order.product");

    protected override bool AllowDelete(Product product) // +Edit
        => HasPermission("admin") || product.MemberCreatedId == CurrentMember!.Id;

    [HttpGet("{id}/Stock")]
    public IActionResult GetStock(long id)
    {
        if (!HasPermission("order.product"))
            NoAccess();
        if (!Repository.Exists(id))
            NotFound();
        var stock = Stock.GetById(id) ?? new Stock { Id = id, Quantity = 0 };
        return Ok(Stock.GetModel(stock));
    }

    [HttpPut("{id}/Stock")]
    public IActionResult UpdateStock(long id, [FromBody] StockData data)
    {
        if (!HasPermission("order.product"))
            NoAccess();
        if (!Repository.Exists(id))
            NotFound();
        var stock = Stock.GetById(id) ?? new Stock { Id = id, Quantity = 0 };
        Stock.Update(stock, data, CurrentMember?.Id);
        return NoContent();
    }
}