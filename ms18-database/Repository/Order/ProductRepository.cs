using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;

namespace Maasgroep.Database.Orders
{
    public class ProductRepository : EditableRepository<Product, ProductModel, ProductData, ProductHistory>, IProductRepository
    {
        protected StockRepository Stock;
        protected Dictionary<long, Stock> ProductStock;
        protected Dictionary<long, long> ProductQuantities;
        
        public ProductRepository(MaasgroepContext db) : base(db)
        {
            Stock = new(db);
            ProductStock = new();
            ProductQuantities = new();
        }

        /** Get memoized product Stock record */
        protected Stock GetProductStock(long id)
        {
            if (ProductStock.ContainsKey(id))
                return ProductStock[id];
            var stock = Stock.GetById(id, true) ?? new();
            ProductStock[id] = stock;
            ProductQuantities[id] = stock.Quantity; // Used to detect stock change
            return stock;
        }

        /** Create ProductModel from Product record */
        public override ProductModel GetModel(Product product)
        {
            var stock = GetProductStock(product.Id);
            return new ProductModel() {
                Id = product.Id,
                Name = product.Name,
                Color = product.Color,
                Icon = product.Icon,
                Price = product.Price,
                PriceQuantity = product.PriceQuantity,
                Stock = stock.Quantity,
            };
        }

        /** Create or update Product record from data model */
        public override Product? GetRecord(ProductData data, Product? existingProduct = null)
        {
            var id = existingProduct?.Id ?? 0;
            var product = existingProduct ?? new();
            product.Name = data.Name;
            product.Color = data.Color ?? "#000000";
            product.Icon = data.Icon ?? "";
            product.Price = data.Price;
            product.PriceQuantity = data.PriceQuantity;
            if (data.Stock != null)
            {
                var stock = GetProductStock(id);
                stock.Quantity = data.Stock ?? 0;
            }
            return product;
        }

        /** Create a ProductHistory record from a Product record */
        public override ProductHistory GetHistory(Product product)
        {
            return new ProductHistory() {
                ProductId = product.Id,
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
                var stock = GetProductStock(record.Id);
                var stockIsNew = stock.Id == 0;
                Console.WriteLine($"PRODUCT: {record.Id}, STOCK: {stock.Id}");
                saveAction.Invoke(db);
                stock.MemberCreatedId ??= record.MemberCreatedId;
                stock.DateTimeDeleted = record.DateTimeDeleted;
                if (stockIsNew)
                {
                    // Stock did not exist before; save the record first, then add the stock with the (likely new) record id
                    var memoizedRecordId = record.Id; // Likely zero, could be non-zero if Product exists in DB without corresponding Stock
                    Console.WriteLine($"ADD NEW STOCK FOR RECORD: {record.Id}");
                    db.SaveChanges();
                    Console.WriteLine($"SAVED RECORD ID: {record.Id}");
                    stock.Id = record.Id;
                    db.Stock.Add(stock);
                    ProductStock.Remove(memoizedRecordId);
                    ProductQuantities.Remove(memoizedRecordId);
                }
                else if (ProductQuantities.ContainsKey(stock.Id) && ProductQuantities[stock.Id] != stock.Quantity)
                {
                    // Stock quantity has changed; update it and create a history record with the old quantity
                    Console.WriteLine($"UPDATE STOCK FOR RECORD: {record.Id}");
                    var stockHistory = Stock.GetHistory(stock with { Quantity = ProductQuantities[record.Id] });
                    ProductQuantities[record.Id] = stock.Quantity;
                    db.StockHistory.Add(stockHistory);
                    db.Stock.Update(stock);
                    ProductStock.Remove(record.Id);
                    ProductQuantities.Remove(record.Id);
                }
            };
        }
    }
}
