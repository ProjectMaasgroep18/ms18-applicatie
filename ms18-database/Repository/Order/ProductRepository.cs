using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;

namespace Maasgroep.Database.Orders
{
    public class ProductRepository : EditableRepository<Product, ProductModel, ProductData, ProductHistory>, IProductRepository
    {
        protected StockRepository Stock;
        public ProductRepository(MaasgroepContext db) : base(db)
            => Stock = new(db);

        /** Create ProductModel from Product record */
        public override ProductModel GetModel(Product product)
        {
            return new ProductModel() {
				Id = product.Id,
				Name = product.Name,
				Color = product.Color,
				Icon = product.Icon,
				Price = product.Price,
                PriceQuantity = product.PriceQuantity,
                Stock = Stock.GetById(product.Id)?.Quantity ?? 0,
			};
        }

		/** Create or update Product record from data model */
        public override Product? GetRecord(ProductData data, Product? existingProduct = null)
        {
            var product = existingProduct ?? new();
			product.Name = data.Name;
			product.Color = data.Color ?? "#000000";
            product.Icon = data.Icon ?? "";
			product.Price = data.Price;
            product.PriceQuantity = data.PriceQuantity;
			return product;
        }

		/** Create a ProductHistory record from a Product record */
        public override ProductHistory GetHistory(Product product)
        {
            return new ProductHistory() {
				Id = product.Id,
				Name = product.Name,
				Color = product.Color,
				Icon = product.Icon,
				Price = product.Price,
                PriceQuantity = product.PriceQuantity,
			};
        }

        /** Ensure stock is created when saving product */
        public override Action<MaasgroepContext> GetSaveAction(Product record)
        {
            var saveAction = base.GetSaveAction(record);
            return (MaasgroepContext db) => {
                saveAction.Invoke(db);
                var stock = Stock.GetById(record.Id);
                if (stock == null)
                {
                    db.Stock.Add(new Stock() { Id = record.Id, MemberCreatedId = record.MemberCreatedId, DateTimeDeleted = record.DateTimeDeleted });
                }
                else if (record.DateTimeDeleted != null && stock?.DateTimeDeleted != null)
                {
                    stock.DateTimeDeleted = record.DateTimeDeleted;
                    db.Stock.Update(stock);    
                }
            };
        }
    }
}
