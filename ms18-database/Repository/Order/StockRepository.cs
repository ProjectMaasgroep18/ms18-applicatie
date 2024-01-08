using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;

namespace Maasgroep.Database.Orders
{
    public class StockRepository : EditableRepository<Stock, StockModel, StockData, StockHistory>, IStockRepository
    {
        public StockRepository(MaasgroepContext db) : base(db) {}

        /** Create StockModel from Stock record */
        public override StockModel GetModel(Stock stock)
        {
            return new StockModel() {
                Id = stock.Id,
                Quantity = stock.Quantity,
            };
        }

        /** Create or update Stock record from data model */
        public override Stock? GetRecord(StockData data, Stock? existingStock = null)
        {
            if (existingStock == null)
                return null; // Stock is created as part of product
            existingStock.Quantity = data.Quantity;
            return existingStock;
        }

        /** Create a StockHistory record from a Stock record */
        public override StockHistory GetHistory(Stock stock)
        {
            return new StockHistory() {
                ProductId = stock.Id,
                Quantity = stock.Quantity,
            };
        }

        /** Override action that will save record changes to DB: make sure Id is kept (because it is the same as Product Id) */
        public override Action<MaasgroepContext> GetSaveAction(Stock record)
        {
            return (MaasgroepContext db) => {
                if (!db.Stock.Any(s => s.Id == record.Id)) {
                    Console.WriteLine($"INSERT new record in {this}");
                    db.Stock.Add(record);
                } else {
                    Console.WriteLine($"UPDATE record {record.Id} in {this}");
                    db.Stock.Update(record);
                }
            };
        }
    }
}
