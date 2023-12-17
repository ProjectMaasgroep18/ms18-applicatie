using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;

namespace Maasgroep.Database.Orders
{
    public class ProductRepository : EditableRepository<Product, ProductModel, ProductData, ProductHistory>, IProductRepository
    {
        public ProductRepository(MaasgroepContext db) : base(db) {}

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
    }
}